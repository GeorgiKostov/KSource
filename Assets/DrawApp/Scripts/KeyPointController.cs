using TMPro;
using UnityEngine;

namespace Assets.DrawApp.Scripts
{
    public class KeyPointController : MonoBehaviour
    {
        [SerializeField] private TextMeshPro keyPointIndexText;
        
        public void LoadKeyPoint(int index)
        {
            this.keyPointIndexText.text = $"{index}";
        }

        public void Dispose()
        {
            Destroy(this.gameObject);
        }
    }
}