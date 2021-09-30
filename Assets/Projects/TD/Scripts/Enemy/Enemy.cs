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
public class Enemy : MonoBehaviour, IDamageble
{
	public EnemyType type;
	public float health = 100;
	public float maxHealth = 100;
	public float remainingDistance;

	[SerializeField] private NavMeshAgent agent;
	private Material mat;

	public event System.Action<Enemy> OnEnemyDied;
	void Start()
	{
		health = maxHealth;
		mat = new Material(GetComponent<MeshRenderer>().sharedMaterial);
		GetComponent<MeshRenderer>().material = mat;
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
		Destroy(gameObject);
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

	
}
