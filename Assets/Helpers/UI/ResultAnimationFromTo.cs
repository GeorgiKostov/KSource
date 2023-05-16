using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Helpers.UI
{
    public class ResultAnimationFromTo : MonoBehaviour
    {
        public Image Background;
        public Sprite WrongSprite;
        public Sprite CorrectSprite;
        public float MovementSpeed;
        public Image ResultImage;
        public TextMeshProUGUI ResultPoints;
        public Vector3 StartPos = new Vector3(0, -300f, 0);
        public Vector3 TargetPos = new Vector3(0, 0f, 0);
        private RectTransform rect;

        [SerializeField]
        private Color successColor;
        [SerializeField]
        private Color failColor;

        private bool state;

        public void Init(bool result, int amount)
        {
            if (result)
            {
                this.ResultImage.sprite = this.CorrectSprite;
                this.Background.color = this.successColor;
            }
            else
            {
                this.ResultImage.sprite = this.WrongSprite;
                this.Background.color = this.failColor;
            }

            this.state = result;
           // DOTween.To(this.AnimateScore, 0, amount, this.MovementSpeed);
          //  this.GetComponent<RectTransform>().DOAnchorPos(this.TargetPos, this.MovementSpeed).OnComplete(this.DoPunch);
        }

        private void AnimateScore(float value)
        {
            this.ResultPoints.text = value.ToString("F0");
        }

        private void DoPunch()
        {
           // this.rect.DOPunchScale(new Vector3(.5f, .5f, .5f), 1f, 0, 0).OnComplete(this.DoScaleDown);
        }

        private void DoScaleDown()
        {
           // this.rect.DOScale(Vector3.zero, .5f).OnComplete(this.Destroy);
        }

        private void Destroy()
        {
            Destroy(this.gameObject);
        }

        private void Start()
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            this.rect = this.GetComponent<RectTransform>();
            this.rect.anchoredPosition3D = this.StartPos;
        }
    }
}