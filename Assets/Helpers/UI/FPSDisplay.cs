using TMPro;
using UnityEngine;

namespace Assets.Scripts.Helpers.UI
{
    public class FPSDisplay : MonoBehaviour
    {
        float deltaTime = 0.0f;
        public TextMeshProUGUI FPSText;
        private float fps;

        public float Fps => this.fps;

        private void Start()
        {
            //this.InvokeRepeating(nameof(AdjustResolution), 5f, 5f);
        }

        void Update()
        {
            this.deltaTime += (Time.deltaTime - this.deltaTime) * 0.1f;
            float msec = this.deltaTime * 1000.0f;
            this.fps = 1.0f / this.deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, this.Fps);
            this.FPSText.text = text + " \n" + Screen.currentResolution;

        }

        private void AdjustResolution()
        {
            if (this.fps < 30 && QualitySettings.resolutionScalingFixedDPIFactor > .5f)
            {
                QualitySettings.resolutionScalingFixedDPIFactor -= .1f;
            }
        }
    }
}