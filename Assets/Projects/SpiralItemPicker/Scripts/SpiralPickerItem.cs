using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpiralPicker
{
    public class SpiralPickerItem : MonoBehaviour
    {
        [SerializeField] private Transform _itemImageContainer;
        [SerializeField] private CanvasGroup _canvasGroup;
        public void CompensateForParentRotation(float angle)
        {
            _itemImageContainer.localRotation = Quaternion.Euler(0, 0, -angle);
        }

        public void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }
    }
}
