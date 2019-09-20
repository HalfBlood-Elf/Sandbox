using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace MemoryTraining
{
    public class Cell : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public float f;

        [SerializeField]
        public Text text;
        [SerializeField]
        Color wrong, right;
        public bool dragable;
        private Vector3 startPos;
        private Transform startTransform;
        private Outline outline;

        public byte number;
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (dragable)
            {
                GetComponent<Image>().raycastTarget = false;
                startPos = transform.position;
                startTransform = transform.parent;
                transform.SetParent(transform.parent.parent.parent.parent);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(dragable)
                transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (dragable)
            {
                transform.position = startPos;
                GetComponent<Image>().raycastTarget = true;

                transform.SetParent(startTransform);
            }
        }
        public void OnDrop(PointerEventData eventData)
        {
            Cell droppedCell = eventData.pointerDrag.GetComponent<Cell>();
            if (droppedCell.dragable&&number == 0 &&!dragable)
            {
                

                number = droppedCell.number;
                Destroy(droppedCell.gameObject);
                text.text = number.ToString();
                text.enabled = true;
            }
        }

        public void Right()
        {
            outline.effectColor = right;
        }
        public void Wrong()
        {
            outline.effectColor = wrong;
        }
        public void SetNumber(byte v)
        {
            number = v;
            text.text = v.ToString();
            if (outline)
                outline.effectColor = new Color(0, 0, 0, 0);
            else
            {
                outline = GetComponent<Outline>();
                outline.effectColor = new Color(0, 0, 0, 0);
            }
        }
        
        void Start()
        {
            outline = GetComponent<Outline>();
            
        }


        // Update is called once per frame
        void Update()
        {
        }
    }
}
