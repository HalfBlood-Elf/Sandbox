using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class Pooler : MonoBehaviour
{
	[SerializeField, SerializeReference] private PoolObject poolPrefab;
	[SerializeField] private uint preLoadItemsCount = 20;
	private List<PoolObject> pool = new List<PoolObject>();

	private void Awake()
	{
		for (int i = 0; i < preLoadItemsCount; i++)
		{
			SpawnNewObjectToPool();
		}
	}

	private PoolObject SpawnNewObjectToPool()
	{
		var itemGo = Instantiate(poolPrefab.gameObject, transform);
		var item = itemGo.GetComponent<PoolObject>();
		item.isAvailableForSpawn = true;
		item.Pool = this;
		pool.Add(item);
		return item;
	}

	public PoolObject GetFirstAvailableObject()
	{
		var firstAvailable = pool.FirstOrDefault(x => x.isAvailableForSpawn);
		if(firstAvailable == null)
		{
			firstAvailable = SpawnNewObjectToPool();
		}
		firstAvailable.isAvailableForSpawn = false;
		return firstAvailable;
	}

	public GameObject SpawnFirstAvailableObject(Vector3 position, Quaternion rotation)
	{
		var obj = GetFirstAvailableObject();
		obj.transform.position = position;
		obj.transform.rotation = rotation;
		obj.Spawned();
		return obj.gameObject;
	}


	public void ReturnToPool(PoolObject obj)
	{
		obj.isAvailableForSpawn = true;
		obj.transform.parent = transform;
		obj.Despawned();
	}


	public abstract class PoolObject: MonoBehaviour
	{
		public abstract bool isAvailableForSpawn { get; set; }
		public abstract Pooler Pool { get; set; }
		public abstract void Spawned();
		public abstract void Despawned();
	}

//#if UNITY_EDITOR
//	[CustomEditor(typeof(Pooler))]
//	public class MyScriptInspector : Editor
//	{
//		SerializedProperty poolPrefabProperty;
//		Pooler script;
//		private void OnEnable()
//		{
//			//script = (Pooler)target;
//			poolPrefabProperty = serializedObject.FindProperty(nameof(script.poolPrefab));
//		}

//		public override void OnInspectorGUI()
//		{
//			script.poolPrefab = EditorGUILayout.ObjectField(script.poolPrefab , typeof(IPoolObject), true);
//			DrawDefaultInspector();
//		}
//	}
//#endif
}
