using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpiralPicker
{
	public class SpiralPicker : MonoBehaviour
	{
		public enum SlotFacingDirection
		{
			UP = 270,
			DOWN = 90,
			LEFT = 180,
			RIGHT = 0
		}

		[SerializeField] private GameObject slotPrefab;
		[SerializeField] private float radius;
		[SerializeField] private float paddingGrad;
		[SerializeField] private int slotsCount = 1;
		[SerializeField] private SlotFacingDirection slotFacingDirection;
		private Transform[] slots;
		private float startAngle = 90 * Mathf.Deg2Rad;
		private void Start()
		{
			slots = new Transform[slotsCount];
			for (int i = 0; i < slotsCount; i++)
			{
				var go = Instantiate(slotPrefab, transform);
				slots[i] = go.transform;
			}
			var radAngle = (-paddingGrad * Mathf.Deg2Rad) * 0 + startAngle;
			Debug.Log(radius * Mathf.Cos(radAngle));
			radAngle = (-paddingGrad * Mathf.Deg2Rad) * 6 + startAngle;
			Debug.Log(radius * Mathf.Cos(radAngle));
		}

		private void Update()
		{
			for (int i = 0; i < transform.childCount; i++)
			{ 
				var radAngle = (-paddingGrad * Mathf.Deg2Rad) * i + startAngle;
				var slot = transform.GetChild(i);
				slot.localPosition = new Vector3(radius * Mathf.Cos(radAngle), radius * Mathf.Sin(radAngle));
				var lookDirection = slot.position - transform.position;
				slot.rotation = FaceObject(slot.position, transform.position, slotFacingDirection);
			}
		}


		public static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition, SlotFacingDirection facing)
		{
			Vector2 direction = targetPosition - startingPosition;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			angle -= (float)facing;
			return Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}
