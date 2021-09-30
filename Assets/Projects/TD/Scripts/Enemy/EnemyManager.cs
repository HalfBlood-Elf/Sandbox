using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public Transform endWaypoint;

	[SerializeField] private List<Enemy> enemies = new List<Enemy>();

	//wave manager

	public void AddEnemy(Enemy enemy)
	{
		enemies.Add(enemy);
		enemy.SetDestination(endWaypoint.position);
		enemy.OnEnemyDied += (e) => enemies.Remove(e);
	}
	
}
