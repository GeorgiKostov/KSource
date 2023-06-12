using TMPro;
using UnityEngine;

namespace Assets.DrawApp.Scripts
{
    public class KeyPointController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro keyPointIndexText;
        [SerializeField] private GameObject visuals;
        
        public void LoadKeyPoint(int index)
        {
            this.keyPointIndexText.text = $"{index}";
        }

        public void Dispose()
        {
            Destroy(this.gameObject);
        }

        public void Hide()
        {
            this.visuals.SetActive(false);
        }

        public void Show()
        {
            this.visuals.SetActive(true);
        }
    }
}