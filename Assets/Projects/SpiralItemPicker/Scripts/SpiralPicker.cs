using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

			//var radAngle = (-paddingGrad * Mathf.Deg2Rad) * 0 + startAngle;
			////Debug.Log(radius * Mathf.Cos(radAngle));
			//radAngle = (-paddingGrad * Mathf.Deg2Rad) * 6 + startAngle;
			//Debug.Log(radius * Mathf.Cos(radAngle));
		}

		//private void Update()
		//{
		//	//PlaceSlots(); //to setup parameters
		//}

		private void PlaceSlots()
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

		private void SpawnSlotsHolders(
			int slotsCount,
			System.Func<Transform, GameObject> spawnCallback)
		{
			slots = new Transform[slotsCount];
			for (int i = 0; i < slotsCount; i++)
			{
				var go = spawnCallback(transform);
				slots[i] = go.transform;
			}
		}
		
		private void ClearSpawnedSlots(System.Action<GameObject> destroyHandler)
		{
			//Undo.RecordObject(towerCreator, "Delete Created Floors");
			var childs = transform.Cast<Transform>().ToArray();

			for (int i = 0; i < childs.Length; i++)
			{
				destroyHandler(childs[i].gameObject);
			}
		}

		public static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition, SlotFacingDirection facing)
		{
			Vector2 direction = targetPosition - startingPosition;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			angle -= (float)facing;
			return Quaternion.AngleAxis(angle, Vector3.forward);
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
							ts[i].slotsCount,
							(parent) => CreatePrefab(ts[i].slotPrefab, parent));
						ts[i].PlaceSlots();
						Undo.CollapseUndoOperations(undoGroupIndex);
					}
				}
			}

			private GameObject CreatePrefab(GameObject prefab, Transform parent)
			{
				var go = PrefabUtility.InstantiatePrefab(prefab, parent) as GameObject;
				Undo.RegisterCreatedObjectUndo(go, "Create radial menu slot");
				return go;
			}

			private void Destroy(GameObject go)
			{
				Undo.DestroyObjectImmediate(go);
			}

		}
#endif
	}
}
