using System.Collections.Generic;
using Assets.DrawApp.Scripts.Letters;
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
        [SerializeField] private float allowedDistance = .5f;
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
            ResetTask();
            GenerateDrawingGoal(obj);
        }

        /// <summary>
        /// Clean up drawing
        /// </summary>
        private void ResetTask()
        {
            this.strokesToDo.ForEach(x=>x.Dispose());
            this.strokesToDo.Clear();
            this.currentStrokeIndex = 0;
            //TODO: reset current drawing, despawn lines etc.
        }

        private void ResetDrawing()
        {
            this.dragPositions.Clear();
            this.drawRenderer.positionCount = 1;
            this.drawRenderer.SetPosition(0, Vector3.zero);
            this.canDraw = false;
            
            // if there are uncompleted segments, reset entire stroke
            if(this.currentStroke.CheckSegmentsLeft())
                this.currentStroke.ResetSegments();
        }
        
        /// <summary>
        ///  Define the strokes, key points and draw the outlines.
        /// </summary>
        /// <param name="data">The letter data</param>
        private void GenerateDrawingGoal(LetterData data)
        {
            StrokeController.KeyPointIndex = 0;

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
                this.feedbackText.text = $"Try a new letter!";
                return;
            }
            
            this.currentStroke = this.strokesToDo[this.currentStrokeIndex];
            this.currentStroke.ShowKeypoints();
            this.currentSegment = currentStroke.GetCurrentSegment();
            
            if (this.currentSegment.State == SegmentState.Init)
            {
                float distanceToStart = Vector3.Distance(this.currentDragPos, currentSegment.Start);

                if (distanceToStart < this.allowedDistance && this.currentSegment.State == SegmentState.Init)
                {
                    this.currentSegment.State = SegmentState.Started;
                    this.feedbackText.text = $"Start...";
                }
            }
            else if (this.currentSegment.State == SegmentState.Started)
            {
                float distanceToLine = DistanceFromPointToLine(currentSegment.Start, currentSegment.End, this.currentDragPos);

                // Check if user is close to the line
                if (distanceToLine > this.allowedDistance && currentSegment.State == SegmentState.Started)
                {
                    this.feedbackImage.color = Color.red;
                    this.feedbackText.text = $"Start again!";
                    ResetDrawing();
                    // Inform user and restart drawing session    
                }
                else
                {
                    this.feedbackText.text = $"";
                    this.feedbackImage.color = Color.green;
                }
                
                // Check distance to segment end and resolve
                float distanceToEnd = Vector3.Distance(this.currentDragPos, currentSegment.End);
                if (distanceToEnd < this.allowedDistance)
                {
                    this.currentStroke.FinishSegment();
                    // if no segments left, go to next stroke or end
                    if (!this.currentStroke.CheckSegmentsLeft())
                    {
                        //mark segment ended, switch to next segment or stroke
                        if (this.currentStrokeIndex < this.strokesToDo.Count)
                        {
                            this.currentStroke.HideKeypoints();
                            this.feedbackText.text = $"Stroke finished!";
                            this.currentStrokeIndex++;
                        }
                        else
                        {
                            // if no more strokes, mark letter as finished
                            this.feedbackImage.color = Color.blue;
                        }
                    }
                    else
                    {
                        this.feedbackText.text = $"Keep drawing...";
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
