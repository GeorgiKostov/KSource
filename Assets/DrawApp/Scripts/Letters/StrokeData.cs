using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DrawApp.Scripts.Letters
{
    [Serializable]
    public struct StrokeData
    {
        public StrokeType Type;
        public List<KeyPoint> KeyPoints;
    }
}