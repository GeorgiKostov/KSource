using System;
using System.Collections.Generic;
using Assets.DrawApp.Scripts.Letters;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.DrawApp.Scripts
{
    public class LetterManager:Singleton<LetterManager>
    {
        [SerializeField] private List<LetterData> letters = new List<LetterData>();

        public static event Action<LetterData> OnLetterSelected;
        public List<LetterData> Letters
        {
            get => this.letters;
            set => this.letters = value;
        }

        public void SelectLetter(string letter)
        {
            var result = this.letters.Find((data => data.Letter == letter));
            if (result != null)
            {
                OnLetterSelected?.Invoke(result);
            }
            else
            {
                Debug.LogWarning("No such letter found.");
            }
        }

        public bool GetLetters(out List<string> listLetters)
        {
            List<string> allLetters = new List<string>();
            foreach (LetterData letter in this.letters)
            {
                allLetters.Add(letter.Letter);
            }
            listLetters = allLetters;

            if (allLetters.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}