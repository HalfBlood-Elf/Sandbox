using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
namespace ColorPicker
{
    public class ColorCellClick : MonoBehaviour, IPointerClickHandler
    {
        private Color color;
        private ColorPicker picker;
        private Image image;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (picker)
                picker.SetColorRGB(color.r, color.g, color.b);
            else
            {
                picker = GameObject.Find("Panel").GetComponent<ColorPicker>();
            }
        }

        // Use this for initialization
        void Start()
        {
            image = GetComponent<Image>();
            picker = GameObject.Find("Panel").GetComponent<ColorPicker>();
        }

        // Update is called once per frame
        void Update()
        {
            color = image.color;
        }
    }
}
