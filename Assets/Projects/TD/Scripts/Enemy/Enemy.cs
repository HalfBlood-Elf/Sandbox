using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType
{
	NoType = 0,
	Light = 1,
	Fast = 2,
	Tank = 3
}
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : Pooler.PoolObject, IDamageble
{
	public event System.Action<Enemy> OnEnemyDied;
	public float RemainingDistance { get => remainingDistance; }
	public float MaxHeath { get => maxHealth; }
	public float CurrentHeath { get => health; }
	public override bool isAvailableForSpawn { get; set; }
	public override Pooler Pool { get; set; }

	public EnemyType type;

	[SerializeField] private float health = 100;
	[SerializeField] public float maxHealth = 100;
	[SerializeField] private float remainingDistance;
	[SerializeField] private NavMeshAgent agent;
	private Material mat;
	private bool startDone;
	private void Start()
	{
		if (startDone) return;
		var meshRenderer = GetComponent<MeshRenderer>();
		mat = new Material(meshRenderer.sharedMaterial);
		meshRenderer.material = mat;
		startDone = true;
	}

	void FixedUpdate()
	{
		//if (Input.GetMouseButtonDown(1))
		//{
		//    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		//    if (Physics.Raycast(ray, out hit, 1000,layers))
		//    {
		//        agent.SetDestination(hit.point);
		//    }
		//}
		if (health > 0 && agent.enabled)
		{
			remainingDistance = agent.remainingDistance;
		}
	}

	public void SetDestination(Vector3 target)
	{
		agent.SetDestination(target);
	}

	private void Die()
	{
		OnEnemyDied?.Invoke(this);
		Pool.ReturnToPool(this);
	}

	public void ApplyDamage(float dmg, System.Action<float> onDmgDone, System.Action<int> onKill)
	{
		health -= dmg;
		mat.color = Color.Lerp(Color.red, Color.green, health / maxHealth);

		if (health <= 0)
		{
			health = 0;
			onKill?.Invoke(1);
			Die();
		}
		else if (health > maxHealth)
			health = maxHealth;

	}

	public override void Spawned()
	{
		if (!startDone)
			Start();
		gameObject.SetActive(true);
		agent.enabled = true;
		health = maxHealth;
		mat.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
	}

	public override void Despawned()
	{
		gameObject.SetActive(false);
		agent.enabled = false;
	}

}
