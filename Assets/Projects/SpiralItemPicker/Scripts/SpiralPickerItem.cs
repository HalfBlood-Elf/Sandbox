using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralPickerItem : MonoBehaviour
{
    [SerializeField] private Transform itemImageContainer;
    [SerializeField] private CanvasGroup canvasGroup;
    public void CompensateForParentRotation(float angle)
    {
        itemImageContainer.localRotation = Quaternion.Euler(0, 0, -angle);
    }

    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
}
