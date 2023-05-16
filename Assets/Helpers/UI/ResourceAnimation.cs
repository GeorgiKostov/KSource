using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers.UI
{
    public class ResourceAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private TextMeshProUGUI amountText;

        private RectTransform rect;

        private Vector2 endPos;

        private void Awake()
        {
            this.rect = this.GetComponent<RectTransform>();
        }

        public void Initialize(Sprite sprite, int amount, Vector3 startPos)
        {
            this.image.sprite = sprite;
            this.amountText.text = $"{amount:+##,##0;-##,##0;0}";
            this.rect.localScale = new Vector3(2, 2, 2);

            // Animate pos
            this.endPos = new Vector2(startPos.x, startPos.y - 300f);
            this.rect.anchoredPosition = startPos;
            this.image.DOFade(0, 5f);
            this.amountText.DOFade(0, 5f);
            this.rect.DOScale(Vector3.one, 5f);
            this.rect.DOAnchorPos(this.endPos, 5f)
                    .OnComplete(this.OnAnchorPosComplete);
        }

        public void Initialize(Sprite sprite, float amount, Vector3 startPos)
        {
            this.image.sprite = sprite;
            this.amountText.text = $"{amount:+##,##0;-##,##0;0}";

            this.rect.localScale = new Vector3(2, 2, 2);
            // Animate pos
            this.endPos = new Vector2(startPos.x, startPos.y - 300f);
            this.rect.anchoredPosition = startPos;
            this.image.DOFade(0, 5f);
            this.amountText.DOFade(0, 5f);
            this.rect.DOScale(Vector3.one, 5f);
            this.rect.DOAnchorPos(this.endPos, 5f)
                .OnComplete(this.OnAnchorPosComplete);
        }

        private void OnAnchorPosComplete()
        {
            this.image.DOFade(1, 0);
            this.amountText.DOFade(1, 0);
        }
    }
}