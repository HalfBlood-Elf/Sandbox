using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace IncomeGame
{
    public class TowerDragging : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
    {
        private CursorChanger cursorChanger;
        private Image image;
        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            var parent = transform.parent;
            transform.SetParent(null);
            transform.SetParent(parent);
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (cursorChanger)
                cursorChanger.SetActiveCursor();
            else
                cursorChanger = EventSystem.current.GetComponent<CursorChanger>().instance;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cursorChanger.SetDefaultCursor();
        }
        void Start()
        {
            cursorChanger = EventSystem.current.GetComponent<CursorChanger>().instance;
            image = GetComponent<Image>();
        }
    }
}