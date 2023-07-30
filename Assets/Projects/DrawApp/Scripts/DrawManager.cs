using System.Collections.Generic;
using Assets.DrawApp.Scripts.Letters;
using Assets.DrawApp.Scripts.Stroke;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.DrawApp.Scripts
{
    public class DrawManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject strokePrefab;
        [SerializeField] private TextMeshProUGUI feedbackText;
        [SerializeField] private Image feedbackImage;
        [SerializeField] private LineRenderer drawRenderer;
        [SerializeField] private float tolerance = .5f;
        [SerializeField] private float endTolerance = .2f;

        private List<StrokeController> strokesToDo = new List<StrokeController>();
        private List<Vector3> dragPositions = new List<Vector3>();
        private Vector3 currentDragPos;
        private StrokeController currentStroke;
        private Segment currentSegment;
        private int currentStrokeIndex = 0;
        private bool canDraw;
        
        private void Awake()
        {
            LetterManager.OnLetterSelected += LetterManagerOnOnLetterSelected;
        }

        private void OnDestroy()
        {
            LetterManager.OnLetterSelected -= LetterManagerOnOnLetterSelected;
        }

        private void LetterManagerOnOnLetterSelected(LetterData obj)
        {
            StrokeController.KeyIndex = 0;
            ResetTask();
            GenerateDrawingGoal(obj);
        }

        /// <summary>
        /// Clean up current task
        /// </summary>
        private void ResetTask()
        {
            this.strokesToDo.ForEach(x=>x.Dispose());
            this.strokesToDo.Clear();
            this.currentStrokeIndex = 0;
        }

        /// <summary>
        /// Clean up drawing
        /// </summary>
        private void ResetDrawing()
        {
            this.dragPositions.Clear();
            this.drawRenderer.positionCount = 1;
            this.drawRenderer.SetPosition(0, Vector3.zero);
            this.canDraw = false;
            
            // if there are uncompleted segments, reset entire stroke
            if (this.currentStroke.IsStrokeComplete())
            {
                this.currentStroke.ResetSegments();
                this.feedbackText.text = $"Please try again!";
            }
        }
        
        /// <summary>
        ///  Define the strokes, key points and draw the outlines.
        /// </summary>
        /// <param name="data">The letter data</param>
        private void GenerateDrawingGoal(LetterData data)
        {
            foreach (StrokeData stroke in data.Strokes)
            {
                var newStroke = Instantiate(this.strokePrefab, transform.position, Quaternion.identity);
                var strokeController = newStroke.GetComponent<StrokeController>();
                strokeController.Initialize(stroke);
                this.strokesToDo.Add(strokeController);
            }
            this.strokesToDo[0].ShowKeypoints();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.canDraw = true;
            this.feedbackText.text = string.Empty;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!this.canDraw) return;
            this.currentDragPos = this.ConvertPointerToWorldPos(eventData.position);
            this.dragPositions.Add(currentDragPos);
            ProcessStroke();
            RenderDrawLine();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            this.ResetDrawing();
        }

        private void RenderDrawLine()
        {
            this.drawRenderer.positionCount = this.dragPositions.Count;
            this.drawRenderer.SetPositions(this.dragPositions.ToArray());
        }
        
        private void ProcessStroke()
        {
            if (this.currentStrokeIndex >= this.strokesToDo.Count)
            {
                this.feedbackImage.color = Color.green;
                this.feedbackText.text = $"Try a new letter!";
                return;
            }
            
            this.currentStroke = this.strokesToDo[this.currentStrokeIndex];
            this.currentStroke.ShowKeypoints();
            this.currentSegment = currentStroke.GetCurrentSegment();
            
            if (this.currentSegment.State == SegmentState.Init)
            {
                float distanceToStart = Vector3.Distance(this.currentDragPos, currentSegment.Start);

                if (distanceToStart < this.tolerance && this.currentSegment.State == SegmentState.Init)
                {
                    this.currentSegment.State = SegmentState.Started;
                }
            }
            else if (this.currentSegment.State == SegmentState.Started)
            {
                float distanceToLine = 0;
                if (this.currentStroke.StrokeData.Type == StrokeType.Linear)
                {
                    distanceToLine = DistanceFromPointToLine(currentSegment.Start, currentSegment.End, this.currentDragPos);
                }
                else if (this.currentStroke.StrokeData.Type == StrokeType.CubicBezier)
                {
                    distanceToLine = this.currentStroke.GetDistanceClosestPointOnCubicBezier(this.currentDragPos);
                }

                // Check if user is close to the line
                if (distanceToLine > this.tolerance && currentSegment.State == SegmentState.Started)
                {
                    // Inform user and restart drawing session    
                    this.feedbackImage.color = Color.red;
                    this.feedbackText.text = $"Please try again!";
                    ResetDrawing();
                }
                else
                {
                    this.feedbackText.text = string.Empty;
                    this.feedbackImage.color = Color.white;
                }
                
                // Check distance to segment end and resolve
                float distanceToEnd = Vector3.Distance(this.currentDragPos, currentSegment.End);
                if (distanceToEnd < this.endTolerance)
                {
                    this.currentStroke.CompleteSegment();
                    // if no segments left, go to next stroke or end
                    if (!this.currentStroke.IsStrokeComplete())
                    {
                        //mark segment ended, switch to next segment or stroke
                        if (this.currentStrokeIndex < this.strokesToDo.Count)
                        {
                            this.currentStroke.HideKeypoints();
                            this.currentStrokeIndex++;
                        }
                    }
                }
            }
        }

        private Vector3 ConvertPointerToWorldPos(Vector3 inputPos)
        {
            Vector3 convertedPos = inputPos;
            convertedPos = Camera.main.ScreenToWorldPoint(inputPos);
            convertedPos.z = 0;
            return convertedPos;
        }
        
        private float DistanceFromPointToLine(Vector3 A, Vector3 B, Vector3 P)
        {
            Vector3 crossProduct = Vector3.Cross(P - A, P - B);
            float lineLength = Vector3.Distance(A, B);
            float distance = crossProduct.magnitude / lineLength;
            return distance;
        }
    }
}
