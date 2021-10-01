using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolObject : MonoBehaviour
{
	public bool available;
	public event Action OnSpawned;
	public event Action OnDespawned;

	public void Spawned()
	{
		OnSpawned?.Invoke();
	}

	public void Despawned()
	{
		OnDespawned?.Invoke();
	}
}
