using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemySpawner : MonoBehaviour
{
	[SerializeField] private Pooler enemyPool;

	[SerializeField] private float spawnRange;
	[SerializeField] private float OffsetY;
	[SerializeField] private float waypointOffset = 0;
	[SerializeField] private float enemyFrequency;
	[SerializeField] private EnemyManager enemyManager;

	private float timer;

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if (timer > enemyFrequency)
		{
			timer = 0;
			var enemy = Spawn();
			enemyManager.AddEnemy(enemy);
		}

	}
	private Enemy Spawn()
	{

		Vector3 pos = new Vector3(
			Random.Range(transform.position.x - spawnRange / 2, transform.position.x + spawnRange / 2), 
			transform.position.y + OffsetY, 
			Random.Range(transform.position.z - spawnRange / 2, transform.position.z + spawnRange / 2));
		GameObject gO = enemyPool.GetFirstAvailableObject();
		gO.transform.position = pos;
		var enemyScript = gO.GetComponent<Enemy>();

		int type = Random.Range(1, 4);
		Debug.Log("Spawning creep: number " + type + " type " + (EnemyType)type);
		enemyScript.type = (EnemyType)type;

		gO.name = enemyScript.type.ToString();

		return enemyScript;
	}
}
