using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class Tower : MonoBehaviour
{
	public EnemyType priority;
	public float range;
	public float reloadTime = 0.3f;
	public float damage = 10;
	public int kills = 0;

	public GameObject muzzlePref;
	public Transform[] guns;
	[SerializeField] private string enemyTag = "Enemy";
	[SerializeField] private Enemy target;
	[SerializeField] private List<Enemy> enemiesInRange = new List<Enemy>();
	[SerializeField] private Transform towerGun;
	[SerializeField] private Transform rayCastPos;

	[SerializeField] private TriggerEvents triggerEvents;
	[SerializeField] private Transform rangeSphere;
	private float timer = 0;
	private Transform muzzlePos;
	private bool leftGun = true;

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

	void FixedUpdate()
	{
		timer += Time.deltaTime;
		if (timer > 100)
			timer = 100;
		var triggerSphere = triggerEvents.Trigger as SphereCollider;
		triggerSphere.radius = range;
		rangeSphere.localScale = new Vector3(range, range, range);
		//triggerSphere.transform.localScale = new Vector3(range, range, range);
		//
		if (target != null)//looking on target
		{
			towerGun.transform.LookAt(target.transform);
			towerGun.transform.eulerAngles = new Vector3(towerGun.transform.eulerAngles.x, towerGun.transform.eulerAngles.y, towerGun.transform.eulerAngles.z);
			towerGun.transform.localEulerAngles += new Vector3(90, 0, 0);
			Shot(rayCastPos.transform.forward);
		}
		else
		{
			SortEnemies();
		}

	}



	void Shot(Vector3 direction)
	{
		if (timer >= reloadTime)
		{
			RaycastHit hit;
			Ray ray = new Ray(rayCastPos.position, direction);
			Physics.Raycast(ray, out hit);
			Debug.DrawLine(ray.origin, hit.point, Color.green, 0.2f);

			muzzlePos = (leftGun) ? guns[0] : guns[1];
			var hitFlash = Instantiate(muzzlePref, muzzlePos);
			hitFlash.transform.localScale = new Vector3(.1f, .1f, .1f);
			var flash = hitFlash.GetComponent<MuzzleFlash>();
			flash.lifeTime = 0.05f;
			flash.scaleModifier = 0.02f;
			leftGun = !leftGun;
			var damageble = hit.transform.GetComponent<IDamageble>();
			if(damageble != null)
			{
				damageble.ApplyDamage(damage, null, (newKills) => kills += newKills);
			}
			else
			{
				Debug.Log($"tower {name} hit {hit.transform.name}");
			}
			
			timer = 0;
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
