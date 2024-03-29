﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class Tower : MonoBehaviour
{
	public EnemyType priority;
	public float range;
	public float reloadTime = 0.3f;
	public float damage = 10;
	public int kills = 0;

	[SerializeField] private Transform[] guns;
	[SerializeField] private string enemyTag = "Enemy";
	[SerializeField] private Transform towerGun;
	[SerializeField] private Transform rayCastPos;

	[SerializeField] private TriggerEvents triggerEvents;
	[SerializeField] private Transform rangeSphere;

	[SerializeField] private Pooler muzzleFlashPool;
	[SerializeField] private ProjectileSettings projectileSettings;
	[SerializeField] private Pooler bulletsPool;


	private float? lastShotTime = null;
	private bool leftGun = true;
	private Enemy target;
	private List<Enemy> enemiesInRange = new List<Enemy>();

	private void OnEnable()
	{
		triggerEvents.OnTriggerEnterFired += TriggerEvents_OnTriggerEnterFired;
		triggerEvents.OnTriggerExitFired += TriggerEvents_OnTriggerExitFired;

	}
	private void OnDisable()
	{
		triggerEvents.OnTriggerEnterFired -= TriggerEvents_OnTriggerEnterFired;
		triggerEvents.OnTriggerExitFired -= TriggerEvents_OnTriggerExitFired;

	}
	private void TriggerEvents_OnTriggerEnterFired(Collider obj)
	{
		if (obj.CompareTag(enemyTag))
		{
			var enemy = obj.GetComponent<Enemy>();
			enemy.OnEnemyDied += Enemy_OnEnemyDiedInRange;
			enemiesInRange.Add(enemy);
			SortEnemies();
		}
	}

	private void TriggerEvents_OnTriggerExitFired(Collider obj)
	{
		if (obj.CompareTag(enemyTag))
		{
			var enemy = obj.GetComponent<Enemy>();
			enemiesInRange.Remove(enemy);
			if (target == enemy)
			{
				target = null;
				SortEnemies();
			}
		}
	}

	private void Enemy_OnEnemyDiedInRange(Enemy enemy)
	{
		if (target == enemy) target = null;
		enemiesInRange.Remove(enemy);
	}

	private void Update()
	{
		var triggerSphere = triggerEvents.Trigger as SphereCollider;
		triggerSphere.radius = range;
		rangeSphere.localScale = new Vector3(range, range, range);
		//triggerSphere.transform.localScale = new Vector3(range, range, range);
		//
		if (target != null)//looking on target
		{
			towerGun.LookAt(target.transform);
			towerGun.localEulerAngles += new Vector3(90, 0, 0);
			ShootOnce(rayCastPos.forward);
		}
		else
		{
			SortEnemies();
		}

	}

	private void ShootOnce(Vector3 forward)
	{
		if (!lastShotTime.HasValue || Time.time - lastShotTime.Value >= reloadTime)
		{

			var muzzlePos = (leftGun) ? guns[0] : guns[1];
			var hitFlash = muzzleFlashPool.GetFirstAvailableObject();
			hitFlash.transform.parent = muzzlePos;
			hitFlash.transform.localPosition = Vector3.zero;
			hitFlash.transform.localRotation = Quaternion.identity;
			hitFlash.Spawned();
			//hitFlash.transform.localScale = new Vector3(.1f, .1f, .1f);
			//var flash = hitFlash.GetComponent<MuzzleFlash>();

			//ShootRaycast(forward);
			ShotBullet(muzzlePos);

			leftGun = !leftGun;
			lastShotTime = Time.time;
		}
	}

	private void ShotBullet(Transform shotPos)
	{
		var bulletObj = bulletsPool.SpawnFirstAvailableObject(shotPos.position, shotPos.rotation);
		var bullet = bulletObj.GetComponent<Projectile>();
		bullet.Setup(projectileSettings, null, (newKills) => kills += newKills);
	}


	private void ShootRaycast(Vector3 direction)
	{
		RaycastHit hit;
		Ray ray = new Ray(rayCastPos.position, direction);
		Physics.Raycast(ray, out hit);
		Debug.DrawLine(ray.origin, hit.point, Color.green, 0.2f);

		var damageble = hit.transform.GetComponent<IDamageble>();
		if(damageble != null)
		{
			damageble.ApplyDamage(damage, null, (newKills) => kills += newKills);
		}
		else
		{
			Debug.Log($"tower {name} hit {hit.transform.name}");
		}
	}

	private void SortEnemies()
	{
		if (enemiesInRange.Count > 0)
		{
			enemiesInRange = enemiesInRange.OrderBy(e => e.RemainingDistance).ToList();
			
		}
		target = enemiesInRange.Count > 0 ? enemiesInRange[0] : null;
	}

	[System.Serializable]
	private class PriorityOrder
	{
		public enum PriorityType
		{
			None,
			Random, 
			NearestToEnd,
			NearestToTower,
			MoreMaxHealth,
			LessMaxHealth,
			MoreCurrentHealth,
			LessCurrentHealth,
			IsAir,
			IsLand,
			IsHover,
			SpecificEnemy,
		}

		public PriorityType priority;
		public object additionalInfo;
	}
}
