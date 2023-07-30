using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.DrawApp.Scripts.UI
{
    public class LetterSelectDropdown:MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown dropDown;
        
        private void Awake()
        {
            this.GenerateDropdown();
            this.dropDown.onValueChanged.AddListener((OnDropdownValueChanged));
        }

        private void Start()
        {
            // Auto select first option
            OnDropdownValueChanged(0);
        }

        public void GenerateDropdown()
        {
            List<string> entries = new List<string>();
            if (LetterManager.Instance.GetLetters(out entries))
            {
                this.dropDown.AddOptions(entries);
            }
            else
            {
                Debug.LogWarning("No Letters Found");
            }
        }
        
        private void OnDropdownValueChanged(int index)
        {
            Debug.Log($"Selected index {index} with letter {this.dropDown.options[index].text}");
            LetterManager.Instance.SelectLetter(this.dropDown.options[index].text);
        }
    }
}