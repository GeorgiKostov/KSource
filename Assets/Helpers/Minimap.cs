using UnityEngine;

//[ExecuteInEditMode]
namespace Assets.Scripts.Helpers
{
    public class Minimap : MonoBehaviour
    {

        // FPS KIT [www.armedunity.com]

        public float minimapSize = 2.0f;
        private float offsetX = 10.0f;
        private float offsetY = 10.0f;
        private float adjustSize = 0.0f;

        public Texture borderTexture;
        public Texture effectTexture;
        public Camera minimapCamera;

        void Start()
        {
            this.minimapCamera.enabled = true;
        }

        void Update()
        {
            this.adjustSize = Mathf.RoundToInt(Screen.width / 10);
            this.minimapCamera.pixelRect = new Rect(this.offsetX, (Screen.height - (this.minimapSize * this.adjustSize)) - this.offsetY, this.minimapSize * this.adjustSize, this.minimapSize * this.adjustSize);
        }

        /// <summary>
        /// Renders the UI
        /// </summary>
        void OnGUI()
        {
            this.minimapCamera.Render();
            GUI.DrawTexture(new Rect(this.offsetX, this.offsetY, this.minimapSize * this.adjustSize, this.minimapSize * this.adjustSize), this.effectTexture);
            GUI.DrawTexture(new Rect(this.offsetX, this.offsetY, this.minimapSize * this.adjustSize, this.minimapSize * this.adjustSize), this.borderTexture);
        }


    }
}