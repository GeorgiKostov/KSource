using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Helpers.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class APanel : MonoBehaviour
    {
        protected CanvasGroup canvas;
        public bool IsShown { get; private set; }

        public virtual void Show()
        {
            if (this.canvas.alpha < 1)
                this.canvas.DOFade(1, 0f);
            this.canvas.blocksRaycasts = true;
            this.canvas.interactable = true;
            this.IsShown = true;
        }

        public virtual void Hide()
        {
            this.canvas.DOFade(0, 0f);
            this.canvas.blocksRaycasts = false;
            this.canvas.interactable = false;
            this.IsShown = false;
        }

        protected virtual void OnDestroy()
        {
            
        }

        protected virtual void Awake()
        {
            this.canvas = this.GetComponent<CanvasGroup>();
        }
    }
}