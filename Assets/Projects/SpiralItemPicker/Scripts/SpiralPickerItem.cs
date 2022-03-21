using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralPickerItem : MonoBehaviour
{
	[SerializeField] private Transform itemImageContainer;

	public void CompensateForParentRotation(float angle)
	{
		itemImageContainer.localRotation = Quaternion.Euler(0,0,-angle);
	}
}
