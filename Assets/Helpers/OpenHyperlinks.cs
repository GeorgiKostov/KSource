using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Helpers
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class OpenHyperlinks : MonoBehaviour, IPointerClickHandler
    {
        private TextMeshProUGUI textMesh;

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click on link");

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(this.textMesh, Input.mousePosition, Camera.main);
            if (linkIndex != -1)
            {
                // was a link clicked?
                TMP_LinkInfo linkInfo = this.textMesh.textInfo.linkInfo[linkIndex];

                // open the link id as a url, which is the metadata we added in the text field
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }

        private void Start()
        {
            this.textMesh = this.GetComponent<TextMeshProUGUI>();
        }
    }
}