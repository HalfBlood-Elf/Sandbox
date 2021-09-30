using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeToLocalScale : MonoBehaviour
{
	[SerializeField] private Tower tower;
	[SerializeField] private float scaleCoeficient;
	[SerializeField] private Transform transformToChange;

	private void Update()
	{
		var scale = tower.range * scaleCoeficient;
		transformToChange.localScale = new Vector3(scale, scale, scale);
	}
}
