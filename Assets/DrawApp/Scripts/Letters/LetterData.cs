using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.DrawApp.Scripts.Letters
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Letter", menuName = "Data/Letter")]
    public class LetterData : ScriptableObject
    {
        [SerializeField] private string letter;
        [SerializeField] private List<StrokeData> strokes = new List<StrokeData>();

        public string Letter
        {
            get
            {
                // In case no letter case was entered, use the name of the scriptable object
                if (this.letter == String.Empty)
                {
                    this.letter = this.name;
                }
                return this.letter;
            }
            set => this.letter = value;
        }

        public List<StrokeData> Strokes
        {
            get => this.strokes;
            set => this.strokes = value;
        }
    }
}
