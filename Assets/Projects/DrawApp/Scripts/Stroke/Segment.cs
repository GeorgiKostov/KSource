using System;
using UnityEngine;

namespace Assets.DrawApp.Scripts.Stroke
{
    [Serializable]
    public class Segment
    {
        public Segment(Vector3 start, Vector3 end)
        {
            this.Start = start;
            this.End = end;
        }
        
        public Vector3 Start;
        public Vector3 End;
        public SegmentState State;
    }
}