using UnityEngine;

namespace Assets.Scripts.Helpers.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaBackgroundOverload : MonoBehaviour
    {
        private void Start()
        {
            RectTransform rect = this.GetComponent<RectTransform>();
            Vector2 screenSize = new Vector2(Screen.width, Screen.height);
            Vector2 safeArea = Screen.safeArea.size;
            //Debug.Log($"Screen size {screenSize} / safeArea: {safeArea}");
            Vector2 overloadSafeArea = new Vector2(screenSize.x - safeArea.x, screenSize.y - safeArea.y);
            rect.sizeDelta = new Vector2(overloadSafeArea.x * 4, overloadSafeArea.y * 4);

            // set as first sibling
            this.transform.SetAsFirstSibling();
        }
    }
}