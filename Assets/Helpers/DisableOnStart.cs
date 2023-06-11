using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public class DisableOnStart:MonoBehaviour
    {
        [SerializeField] private bool disableOnStart;
        
        private void Start()
        {
            if(this.disableOnStart)
                this.gameObject.SetActive(false);
        }
    }
}