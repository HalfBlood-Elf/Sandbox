using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColorPicker
{
    public class S_V_pick : MonoBehaviour,IDragHandler,IPointerDownHandler
    {
        public RectTransform point;
        public float H;

        private Image image;

        private RectTransform rectTransform;
        private ColorPicker picker;

        Vector2 center;
        [SerializeField]
        float width, height;

        public void SetColor(float s,float v)
        {
            Debug.Log(s + " " + v);
            point.localPosition = new Vector2((s*100) - center.x, (v *100) - center.y);
        }


        public void SetH(float h)
        {
            H = h;
            image.color = Color.HSVToRGB(H / 360, 1, 1);
        }
        public void OnDrag(PointerEventData eventData)
        {
            center = rectTransform.position;
            width = rectTransform.sizeDelta.x;
            height = rectTransform.sizeDelta.y;

            var x = Mathf.Clamp(eventData.position.x, center.x - width/2, center.x +width/2 );
            var y = Mathf.Clamp(eventData.position.y, center.y - height/2 ,center.y + height/2);
            point.localPosition = new Vector2(x - center.x, y - center.y);
            picker.SetSV((x - center.x + 50) /100, (y - center.y + 50)/100);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }
        void Start()
        {
            image = GetComponent<Image>();
            picker = transform.parent.GetComponent<ColorPicker>();
            rectTransform = GetComponent<RectTransform>();

        }
        
        void Update()
        {

        }
    }
}
