using System.Collections.Generic;
using Assets.DrawApp.Scripts.Letters;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.DrawApp.Scripts
{
    public class StrokeController:MonoBehaviour
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private Material completedLineMat;
        [SerializeField] private GameObject keyPointPrefab;
        [SerializeField] List<Segment> segments = new List<Segment>();

        private List<KeyPointController> keypoints = new List<KeyPointController>();
        private List<Vector3> keyPointPositions = new List<Vector3>();
        private List<Vector3> controlPointPositions = new List<Vector3>();
        private StrokeData strokeData;
        private int currentSegment;
        public static int KeyPointIndex;

        public List<Vector3> ControlPointPositions => this.controlPointPositions;
        public List<Vector3> KeyPointPositions => this.keyPointPositions;

        public List<Segment> Segments => this.segments;


        public void Initialize(StrokeData data)
        {
            this.strokeData = data;
            
            // Visualize keypoints
            foreach (KeyPoint point in this.strokeData.KeyPoints)
            {
                var newPointViz = Instantiate(this.keyPointPrefab, point.Coordinate, quaternion.identity);
                this.keyPointPositions.Add(point.Coordinate);
                var keyPointController = newPointViz.GetComponent<KeyPointController>();
                keyPointController.LoadKeyPoint(KeyPointIndex);
                this.keypoints.Add(keyPointController);
                KeyPointIndex++;
            }
            
            // Create line points
            if (this.strokeData.Type == StrokeType.QuadraticBezier && this.strokeData.KeyPoints.Count == 3)
            {
                this.controlPointPositions.AddRange(GenerateQuadraticBezierCurve(this.KeyPointPositions[0], this.KeyPointPositions[1],this.KeyPointPositions[2], 50));
            }
            else if (this.strokeData.Type == StrokeType.CubicBezier && this.strokeData.KeyPoints.Count == 4)
            {
                this.controlPointPositions.AddRange(GenerateCubicBezierCurve(this.KeyPointPositions[0], this.KeyPointPositions[1],this.KeyPointPositions[2], this.KeyPointPositions[3], 50));
            }
            else
            {
                for (int i = 0; i < this.KeyPointPositions.Count-1; i++)
                {
                    this.controlPointPositions.AddRange(GenerateLinearBezier(this.KeyPointPositions[i], this.KeyPointPositions[i + 1]));
                }
            }

            // Generate line
            this.line.positionCount = this.controlPointPositions.Count;
            this.line.SetPositions(this.controlPointPositions.ToArray());
            
            // Create logical segments
            for (int i = 0; i < this.keyPointPositions.Count - 1; i++)
            {
                this.segments.Add(new Segment(this.keyPointPositions[i], this.keyPointPositions[i + 1]));
            }
        }

        public Segment GetCurrentSegment()
        {
           return this.segments[this.currentSegment];
        }

        public void FinishSegment()
        {
            this.segments[this.currentSegment].State = SegmentState.Completed;
            this.currentSegment++;
        }
        
        public bool CheckSegmentsLeft()
        {
            if (this.currentSegment < this.segments.Count)
            {
                return true;
            }
            else
            {
                FinishStroke();
                return false;
            }
        }

        private void FinishStroke()
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

        List<Vector3> GenerateQuadraticBezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, int numPoints)
        {
            List<Vector3> points = new List<Vector3>();

            for (int i = 0; i <= numPoints; i++)
            {
                float t = i / (float)numPoints;
                Vector3 point = Mathf.Pow(1 - t, 2) * P0 +
                                2 * (1 - t) * t * P1 +
                                Mathf.Pow(t, 2) * P2;
                points.Add(point);
            }

            return points;
        }
        
        List<Vector3> GenerateCubicBezierCurve(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, int numPoints)
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
    }
}