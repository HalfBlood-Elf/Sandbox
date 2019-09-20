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
public class EnemyControl : MonoBehaviour {
    public EnemyType type;
    public LayerMask layers;
    public float health = 100;
    public float maxHealth = 100;
    public float waypointOffset = 3;
    public float remainingDistance;

    [SerializeField]
    private List<GameObject> waypoints;
    [SerializeField]
    private int currWayp = 0;
    private Ray ray;
    private RaycastHit hit;
    private NavMeshAgent agent;
    private Material mat;
    void Start()
    {
        waypointOffset = 3;

        waypoints = new List<GameObject>(GameObject.FindGameObjectsWithTag("Waypoint"));
        if (waypoints.Count > 0)
        {
            waypoints.Sort(delegate (GameObject a, GameObject b) {
                return (a.name.CompareTo(b.name));
            });
        }
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        if(agent.enabled)
            agent.SetDestination(waypoints[0].transform.position);
        mat = new Material(GetComponent<MeshRenderer>().material);
        GetComponent<MeshRenderer>().material = mat;
    }
	
	void FixedUpdate () {
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        else if (health > maxHealth)
            health = maxHealth;
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
            if (agent.remainingDistance <= waypointOffset && currWayp < waypoints.Count - 1)
            {
                agent.SetDestination(waypoints[++currWayp].transform.position);
            }
            remainingDistance = agent.remainingDistance;
            mat.color = Color.Lerp(Color.red, Color.green, health / maxHealth);
        }
    }
    void ApplyDamage(float dmg)
    {
        health -= dmg;
    }
    void Die()
    {
        foreach(var tower in GameObject.FindGameObjectsWithTag("Tower"))
        {
            tower.SendMessage("Kill", gameObject.transform);

            Debug.Log(gameObject.name + " dead!");
        }
        Destroy(gameObject);
    }
}
