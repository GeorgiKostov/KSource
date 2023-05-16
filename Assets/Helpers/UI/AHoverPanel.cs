using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Helpers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AHoverPanel : MonoBehaviour
    {
        [SerializeField]
        protected GameObject follow;
        protected CanvasGroup canvas;

        [SerializeField]
        private Vector2 offset;

        protected RectTransform rect;

        public virtual void Awake()
        {
            this.canvas = this.GetComponent<CanvasGroup>();
            this.rect = this.GetComponent<RectTransform>();
        }

        public virtual void Update()
        {
            if (this.follow != null)
            {
                // /this.rect.anchoredPosition = Vector3.Lerp(this.rect.anchoredPosition, ScreenManager.Instance.WorldToCanvasPosition(this.follow.transform.position) + this.offset, Time.deltaTime * 25f);
            }
        }

        public virtual void OnDestroy()
        {
        }

        public virtual void Hide()
        {
            this.canvas.DOFade(0, .1f);
            this.canvas.blocksRaycasts = false;
        }

        public virtual void Show()
        {
            this.canvas.DOFade(1, .1f);
            this.canvas.blocksRaycasts = true;
        }
    }
}