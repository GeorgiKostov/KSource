using System.Collections.Generic;
using Assets.DrawApp.Scripts.Letters;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.DrawApp.Scripts.Stroke
{
    public class StrokeController:MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private Material completedLineMat;
        [SerializeField] private GameObject keyPointPrefab;
        [SerializeField] List<Segment> segments = new List<Segment>();

        private List<KeyPointController> keypoints = new List<KeyPointController>();
        private List<Vector3> keyPointPositions = new List<Vector3>();
        private List<Vector3> linePositions = new List<Vector3>();
        private StrokeData strokeData;
        private int currentSegment;
        public StrokeData StrokeData => this.strokeData;
        public static int KeyIndex = 0;

        public void Initialize(StrokeData data)
        {
            this.strokeData = data;
            
            // Add key point positions
            foreach (KeyPoint point in this.strokeData.KeyPoints)
            {
                this.keyPointPositions.Add(point.Coordinate);
            }
            
            // Create points and segments
            if (this.strokeData.Type == StrokeType.CubicBezier && this.strokeData.KeyPoints.Count == 4)
            {
                // add line points
                this.linePositions.AddRange(GenerateCubicBezierCurve(this.keyPointPositions[0], this.keyPointPositions[1],this.keyPointPositions[2], this.keyPointPositions[3], 50));
                
                // add segments
                this.segments.Add(new Segment(this.keyPointPositions[0], this.keyPointPositions[3])); // for cubic bezier use only 1 segment

                // create key point visuals
                List<Vector3> keyPointsOnCubicBezier = new List<Vector3>();
                keyPointsOnCubicBezier.Add(this.keyPointPositions[0]);
                float keyPointsOnBezier = 3;
                float step = 1 / keyPointsOnBezier;
                for (float i = step; i < 1; i += step)
                {
                    keyPointsOnCubicBezier.Add(this.GetPoint(keyPointPositions[0], keyPointPositions[1], keyPointPositions[2], keyPointPositions[3], i));
                }
                keyPointsOnCubicBezier.Add(this.keyPointPositions[3]);
                for(int i = 0; i < keyPointsOnCubicBezier.Count; i++)
                {
                    var newPointViz = Instantiate(this.keyPointPrefab, keyPointsOnCubicBezier[i], quaternion.identity);
                    var keyPointController = newPointViz.GetComponent<KeyPointController>();
                    keyPointController.LoadKeyPoint(KeyIndex);
                    keyPointController.Hide();
                    this.keypoints.Add(keyPointController);
                    KeyIndex++;
                }
            }
            else if(this.strokeData.Type == StrokeType.Linear)
            {
                // add line positions
                for (int i = 0; i < this.keyPointPositions.Count - 1; i++)
                {
                    this.linePositions.AddRange(GenerateLinearBezier(this.keyPointPositions[i], this.keyPointPositions[i + 1]));
                }
                
                // add segments
                for (int i = 0; i < this.keyPointPositions.Count - 1; i++)
                {
                    this.segments.Add(new Segment(this.keyPointPositions[i], this.keyPointPositions[i + 1]));
                }

                // create key point visuals
                for(int i = 0; i < this.strokeData.KeyPoints.Count; i++)
                {
                    var newPointViz = Instantiate(this.keyPointPrefab, this.strokeData.KeyPoints[i].Coordinate, quaternion.identity);
                    var keyPointController = newPointViz.GetComponent<KeyPointController>();
                    keyPointController.LoadKeyPoint(KeyIndex);
                    keyPointController.Hide();
                    this.keypoints.Add(keyPointController);
                    KeyIndex++;
                }
            }

            // Draw example line
            this.line.positionCount = this.linePositions.Count;
            this.line.SetPositions(this.linePositions.ToArray());
            // Simplify line to avoid weird edges
            if(this.strokeData.Type == StrokeType.Linear)
                this.line.Simplify(1);
        }

        public Segment GetCurrentSegment()
        {
           return this.segments[this.currentSegment];
        }

        public void CompleteSegment()
        {
            this.segments[this.currentSegment].State = SegmentState.Completed;
            this.currentSegment++;
        }
        
        public bool IsStrokeComplete()
        {
            if (this.currentSegment < this.segments.Count)
            {
                return true;
            }
            else
            {
                CompleteStroke();
                return false;
            }
        }

        private void CompleteStroke()
        {
            this.line.material = this.completedLineMat;
        }

        public void ResetSegments()
        {
            this.segments.ForEach(x=>x.State = SegmentState.Init);
            this.currentSegment = 0;
        }

        public void Dispose()
        {
            this.keypoints.ForEach(x=>x.Dispose());
            Destroy(this.gameObject);
        }

        public void ShowKeypoints()
        {
            this.keypoints.ForEach(x=>x.Show());    
        }

        public void HideKeypoints()
        {
            this.keypoints.ForEach(x=>x.Hide());
        }
        
        private List<Vector3> GenerateLinearBezier(Vector3 start, Vector3 end)
        {
            List<Vector3> vectors = new List<Vector3>();
            
            for (int i = 0; i <= 10; i++)
            {
                float offset = i / 10f;
                Vector3 output = Vector3.Lerp(start, end, offset);
                vectors.Add(output);
            }

            return vectors;
        }
        
        private List<Vector3> GenerateCubicBezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, int numPoints)
        {
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i <= numPoints; i++)
            {
                float t = i / (float)numPoints;
                Vector3 point = Mathf.Pow(1 - t, 3) * P0 +
                                3 * Mathf.Pow(1 - t, 2) * t * P1 +
                                3 * (1 - t) * Mathf.Pow(t, 2) * P2 +
                                Mathf.Pow(t, 3) * P3;
                points.Add(point);
            }

            return points;
        }
        
        public float GetDistanceClosestPointOnCubicBezier(Vector3 point)
        {
            float closestDistSqr = Mathf.Infinity;
            Vector3 closestPoint = Vector3.zero;
            float sampleRate = 0.01f; // smaller values make the search more accurate but slower

            for (float t = 0; t <= 1; t += sampleRate)
            {
                Vector3 samplePoint = GetPoint(this.keyPointPositions[0], this.keyPointPositions[1],this.keyPointPositions[2], this.keyPointPositions[3], t);
                float distSqr = (samplePoint - point).sqrMagnitude;

                if (distSqr < closestDistSqr)
                {
                    closestDistSqr = distSqr;
                    closestPoint = samplePoint;
                }
            }

            return Vector3.Distance(point, closestPoint);
        }
        
        private Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return
                oneMinusT * oneMinusT * oneMinusT * p0 +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * p3;
        }
    }
}