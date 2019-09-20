using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MemoryTraining
{
    public class SliderText : MonoBehaviour
    {
        public PanelBehavior panel;
        public Text textComponent;

        private Slider slider;
        [SerializeField]
        private bool countDown = false;
        public void SetTextValue(float sliderValue)
        {
            textComponent.text = sliderValue.ToString("f2");
        }
        public void StartCountDown()
        {
            countDown = true;
            slider.interactable = false;
        }
        private void Start()
        {
            slider = GetComponent<Slider>();
        }
        private void Update()
        {
            if (countDown)
            {
                slider.value -= Time.deltaTime;
            }
            if (slider.value == slider.minValue && countDown)
            {
                countDown = false;
                slider.interactable = true;
                panel.SendMessage("HideCells", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
