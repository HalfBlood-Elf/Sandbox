using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpiralPicker
{
	public class SpiralPicker : MonoBehaviour
	{
		private const int ItemsCountBase = 12;

		public enum SlotFacingDirection
		{
			UP = 270,
			DOWN = 90,
			LEFT = 180,
			RIGHT = 0
		}
		[SerializeField] private float startAngleDeg = 90;
		[SerializeField] private Transform slotsContainer;
		[SerializeField] private SpiralPickerItem slotPrefab;

		[SerializeField] private float minRadius = 100;
		[SerializeField] private float maxRadius = 150;
		[SerializeField] private float minScale = .6f;
		[SerializeField] private float maxScale = 1.6f;

		[SerializeField] private float paddingDeg = 30;
		[SerializeField] private float minPaddingDeg = 20;
		[SerializeField] private float maxPaddingDeg = 40;
		[Tooltip("How many items to the left/right of the selected item")]
		[SerializeField] private int wingsSlotsshown = 7;
		[SerializeField] private SlotFacingDirection slotFacingDirection;
		[SerializeField] private SpiralPickerItem[] slots;

		[SerializeField] private bool clockwiseSmaller = true;

		private int selectedSlotIndex = 0;
		


		private RectTransform rectTransform => transform as RectTransform;

		private void Start()
		{

			//var radAngle = (-paddingGrad * Mathf.Deg2Rad) * 0 + startAngle;
			////Debug.Log(radius * Mathf.Cos(radAngle));
			//radAngle = (-paddingGrad * Mathf.Deg2Rad) * 6 + startAngle;
			//Debug.Log(radius * Mathf.Cos(radAngle));
		}

		private void Update()
		{
			PlaceSlots(0); //to setup parameters
		}

		private void UpdateInputDirection()
		{
			Vector3 userInputDirection = (Input.mousePosition - rectTransform.position).normalized;

		}

		private void PlaceSlots(int selectedSlot)
		{
			for (int i = 0; i < slots.Length; i++)
			{
				var indexNormalised = ((float)i) / slots.Length;

				var slot = slots[i];
				var padding = Mathf.Lerp(minPaddingDeg, maxPaddingDeg, indexNormalised);

				var targetAngle = -padding * i + startAngleDeg - (float)slotFacingDirection;

				var radius = Mathf.Lerp(minRadius, maxRadius, indexNormalised);
				var radAngle = targetAngle * Mathf.Deg2Rad;
				slot.transform.localPosition = new Vector3(radius * Mathf.Cos(radAngle), radius * Mathf.Sin(radAngle));

				var rotationZ = FaceObjectAngleDeg(slotsContainer.localPosition, slot.transform.localPosition, slotFacingDirection);
				slot.transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
				slot.CompensateForParentRotation(rotationZ);

				var scaleIndex = Mathf.Lerp(minScale, maxScale, indexNormalised);
				slot.transform.localScale = Vector3.one * scaleIndex;
			}
		}

		private void SpawnSlotsHolders(
			int wingsSlotsshown,
			System.Func<Transform, SpiralPickerItem> spawnCallback)
		{
			var totalSlotsCount = 1 + wingsSlotsshown + wingsSlotsshown;

			slots = new SpiralPickerItem[totalSlotsCount];
			for (int i = 0; i < totalSlotsCount; i++)
			{
				SpiralPickerItem item = spawnCallback(slotsContainer);
				slots[i] = item;
			}
		}
		
		private void ClearSpawnedSlots(System.Action<GameObject> destroyHandler)
		{
			//Undo.RecordObject(towerCreator, "Delete Created Floors");
			var childs = slotsContainer.Cast<Transform>().ToArray();

			for (int i = 0; i < childs.Length; i++)
			{
				destroyHandler(childs[i].gameObject);
			}
		}

		public static float FaceObjectAngleDeg(Vector2 startingPosition, Vector2 targetPosition, SlotFacingDirection facing)
		{
			Vector2 direction = targetPosition - startingPosition;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			angle -= (float)facing;
			return angle;
		}


#if UNITY_EDITOR
		[CustomEditor(typeof(SpiralPicker)), CanEditMultipleObjects]
		public class SpiralPicker_Editor: Editor
		{
			private SpiralPicker[] ts;

			private void OnEnable()
			{
				ts = targets.Cast<SpiralPicker>().ToArray();
			}

			public override void OnInspectorGUI()
			{
				base.OnInspectorGUI();

				if(GUILayout.Button("Recreate slots"))
				{
					for (int i = 0; i < ts.Length; i++)
					{
						Undo.IncrementCurrentGroup();
						Undo.SetCurrentGroupName("Create radial slots");
						var undoGroupIndex = Undo.GetCurrentGroup();

						ts[i].ClearSpawnedSlots((go) => Destroy(go));

						ts[i].SpawnSlotsHolders(
							ts[i].wingsSlotsshown,
							(parent) => CreatePrefab(ts[i].slotPrefab, parent));
						ts[i].PlaceSlots(0);
						Undo.CollapseUndoOperations(undoGroupIndex);
					}
				}
			}

			private SpiralPickerItem CreatePrefab(SpiralPickerItem prefab, Transform parent)
			{
				var go = PrefabUtility.InstantiatePrefab(prefab.gameObject, parent) as GameObject;
				Undo.RegisterCreatedObjectUndo(go, "Create radial menu slot");
				return go.GetComponent<SpiralPickerItem>();
			}

			private void Destroy(GameObject go)
			{
				Undo.DestroyObjectImmediate(go);
			}

		}
#endif
	}
}
