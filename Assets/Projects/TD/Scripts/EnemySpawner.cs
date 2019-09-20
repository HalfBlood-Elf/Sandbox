using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float spawnRange;
    public float OffsetY;
    public float waypointOffset = 0;

    float timer;
    public float enemyFrequency;

    void Start()
    {

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.deltaTime;
        if(timer > enemyFrequency)
        {
            Spawn();
            timer = 0;
        }

	}
    void Spawn()
    {

        Vector3 pos = new Vector3(Random.Range(transform.position.x - spawnRange / 2, transform.position.x + spawnRange / 2),transform.position.y +OffsetY, Random.Range(transform.position.z - spawnRange / 2, transform.position.z + spawnRange / 2));
        GameObject gO = Instantiate(enemyPrefab, pos, Quaternion.FromToRotation(Vector3.forward, transform.forward));
        gO.GetComponent<EnemyControl>().waypointOffset = waypointOffset;

        int type = Random.Range(1, 4);
        Debug.Log("Spawning creep: number " + type+ " type "+ (EnemyType)type);
        gO.GetComponent<EnemyControl>().type = (EnemyType) type;

        gO.name = gO.GetComponent<EnemyControl>().type.ToString();
    }
}
