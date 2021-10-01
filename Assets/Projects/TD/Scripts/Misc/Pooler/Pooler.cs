using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pooler : MonoBehaviour
{
	[SerializeField] private PoolObject poolObject;
	[SerializeField] private uint preLoadItemsCount = 20;
	private List<PoolObject> pool = new List<PoolObject>();

	private void Awake()
	{
		for (int i = 0; i < preLoadItemsCount; i++)
		{
			var item = Instantiate(poolObject, transform);
			item.available = true;
			pool.Add(item);
		}
	}

	public GameObject GetFirstAvailableObject()
	{
		var firstAvailable = pool.FirstOrDefault(x => x.available);
		if(firstAvailable == null)
		{
			firstAvailable = Instantiate(poolObject, transform);
		}
		firstAvailable.Spawned();
		return firstAvailable.gameObject;
	}

	public void ReturnToPool(PoolObject obj)
	{
		obj.available = true;
		obj.Despawned();
	}
}
