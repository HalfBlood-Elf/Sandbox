using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ColorPicker
{
    public class HPick : MonoBehaviour, IPointerDownHandler,IDragHandler
    {
        public List<RectTransform> point = new List<RectTransform>();
        public float radius;
        public int colors;

        private Image image;
        private RectTransform rectTtransform;
        private Vector2 center;

        private S_V_pick svPick;
        private ColorPicker picker;
        private RectTransform pointPos;
        public void OnDrag(PointerEventData eventData)
        {
            center = new Vector2(rectTtransform.position.x, rectTtransform.position.y);

            var HueDirr = eventData.position - center;
            HueDirr.Normalize();

            point[0].localPosition = HueDirr * radius;
            if (colors == 2)
                point[1].localPosition = -HueDirr * radius;
            else if( colors == 3)
            {
                point[1].localPosition = (Quaternion.Euler(0,0,160)* HueDirr) * radius;
                point[2].localPosition = (Quaternion.Euler(0, 0,-160) * HueDirr) * radius;
                //var x = radius * Mathf.Cos(h * Mathf.Deg2Rad);
                //var y = radius * Mathf.Sin(h * Mathf.Deg2Rad);

            }




            float Hf = Mathf.Atan2(0, 1) - Mathf.Atan2(HueDirr.y, HueDirr.x);
            Hf = Mathf.Rad2Deg * Hf;
            int H = Mathf.RoundToInt(Hf);
            if (H < 0)
                H += 360;
            svPick.SetH(H);
            picker.SetH(H);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void SetColor(float h)
        {

            svPick.SetH(h);
            var x = radius * Mathf.Cos(h * Mathf.Deg2Rad);
            var y = radius * Mathf.Sin(h * Mathf.Deg2Rad);
            point[0].localPosition = new Vector2(-x, y);
            x = radius * Mathf.Cos(((h - 180)%360) * Mathf.Deg2Rad);
            y = radius * Mathf.Sin(((h - 180) % 360) * Mathf.Deg2Rad);

            point[1].localPosition = new Vector2(-x, y);
        }
        public void SetSchema(int colors)
        {
            this.colors = colors + 1;
            for (int i = 0; i < 3; i++)
            {
                if (i > colors)
                    point[i].gameObject.SetActive(false);
                else point[i].gameObject.SetActive(true);
            }
        }
        // Use this for initialization
        void Start()
        {
            image = GetComponent<Image>();
            rectTtransform = GetComponent<RectTransform>();
            image.alphaHitTestMinimumThreshold = 1;

            svPick = GameObject.Find("S_And_V").GetComponent<S_V_pick>();
            picker = transform.parent.GetComponent<ColorPicker>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
