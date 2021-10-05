using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Pooler.PoolObject
{
	[SerializeField] private TriggerEvents triggerEvents;
	private int currentHits;
	[SerializeField] private ProjectileSettings settings;


	private bool awakeDone;
	private Action<float> onDmgDone;
	private Action<int> onKill;
	private Coroutine despawnRoutine;
	public override void Despawned()
	{
		gameObject.SetActive(false);
	}

	public override void Spawned()
	{
		if (!awakeDone) Awake();
		currentHits = 0;
		onDmgDone = null;
		onKill = null;
		gameObject.SetActive(true);
		if (despawnRoutine != null) StopCoroutine(despawnRoutine);
		despawnRoutine = StartCoroutine(DespawnAfterLifetime());

	}

	public void Setup(ProjectileSettings settings, Action<float> onDmgDone, Action<int> onKill)
	{
		this.settings = settings;
		this.onDmgDone = onDmgDone;
		this.onKill = onKill;
	}
	private void Awake()
	{
		if (awakeDone) return;
		triggerEvents.OnTriggerEnterFired += TriggerEvents_OnTriggerEnterFired;

		awakeDone = true;
	}

	private void TriggerEvents_OnTriggerEnterFired(Collider obj)
	{
		if (currentHits < settings.maxHits)
		{
			currentHits ++;
			var damageble = obj.GetComponent<IDamageble>();
			if (damageble != null)
			{
				damageble.ApplyDamage(settings.onHitDmg * currentHits * settings.consecutiveHitsDmgMultiplier, onDmgDone, onKill);
			}
		}
		else
		{
			Despawn();
		}
	}

	private void Despawn()
	{
		if (despawnRoutine != null) StopCoroutine(despawnRoutine);
		despawnRoutine = null;
		Pool.ReturnToPool(this);
	}

	private IEnumerator DespawnAfterLifetime()
	{
		yield return new WaitForSeconds(settings.lifeTime);
		Despawn();
	}

	private void Update()
	{
		transform.position += transform.forward * settings.speed * Time.deltaTime;
	}

}
