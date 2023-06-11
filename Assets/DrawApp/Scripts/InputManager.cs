using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.DrawApp.Scripts
{
    public class InputManager:MonoBehaviour, IPointerDownHandler, IPointerClickHandler,
        IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler,
        IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Vector3 touchWorldPos;
        
        public void Update()
        {
            // register mouse events
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.z= Camera.main.nearClipPlane;
                this.touchWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
            }

            if (Input.GetMouseButtonDown(0))
            {
                // start stroke
            }

            if (Input.GetMouseButtonUp(0))
            {
                // end stroke
            }

            
            // Register touch events
            if (Input.touches.Length <= 0) return;
            var firstFinger = Input.GetTouch(0);

            if (firstFinger.phase == TouchPhase.Began)
            {
                
            }else if (firstFinger.phase == TouchPhase.Canceled)
            {
 
            }else if (firstFinger.phase == TouchPhase.Moved)
            {
                    
            }else if (firstFinger.phase == TouchPhase.Ended)
            {
                
            }
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Drag Begin");
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Dragging");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("Drag Ended");
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Clicked: " + eventData.pointerCurrentRaycast.gameObject.name);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("Mouse Down: " + eventData.pointerCurrentRaycast.gameObject.name);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Mouse Enter");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Mouse Exit");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Mouse Up");
        }
    }
}