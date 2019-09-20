using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TowerType
{
    MachineGun,
    RailGun
}

public class Tower : MonoBehaviour {
    public EnemyType priority;
    public TowerType type;
    public float range;
    public float rangeScale = 13;
    public float reloadTime = 0.3f;
    public float damage = 10;
    public uint kills = 0;

    public GameObject muzzlePref;
    public GameObject lazerPref;
    public Transform[] guns;
    private Transform muzzlePos;
    private bool leftGun = true;
    
    private float timer = 0;
    private GameObject[] enemies;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private List<Transform> enemiesInRange;
    private Image towerRangeUI;
    private Vector3 towerRangeScale;
    private GameObject towerGun;
    private Transform rayCastPos;
	void Start () {
        
        range = 2 * rangeScale;
        towerRangeUI = transform.Find("TowerCanvas").Find("TowerRange").GetComponent<Image>();
        towerGun = transform.Find("TowerGun").gameObject;
        rayCastPos = towerGun.transform.Find("RaycastPos");
        enemiesInRange = new List<Transform>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        timer += Time.deltaTime;
        if (timer > 100)
            timer = 100;
        towerRangeScale = new Vector3(range / rangeScale, range / rangeScale, 0); //UI range scale
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(var enemy in enemies)
        {
            if(Vector3.Distance(enemy.transform.position, transform.position) <= range && !enemiesInRange.Contains(enemy.transform))
            {
                enemiesInRange.Add(enemy.transform);
            }
        }


        foreach (var enemy in enemiesInRange.ToArray())
        {
            if (enemy != null)
            {
                if (Vector3.Distance(enemy.position, transform.position) > range)
                {
                    if (target == enemy)
                    {
                        target = null;
                    }
                    enemiesInRange.Remove(enemy);
                    SortEnemies();
                }
            }
            else enemiesInRange.Remove(enemy);
        }

        //set target selection algorithm
        if(enemiesInRange.Count > 0)
        {
            target = enemiesInRange[0];
        }
        
        //
        if (target != null)//looking on target
        {
            towerGun.transform.LookAt(target);
            towerGun.transform.eulerAngles = new Vector3(towerGun.transform.eulerAngles.x, towerGun.transform.eulerAngles.y, towerGun.transform.eulerAngles.z);
            if (type == TowerType.MachineGun)
                towerGun.transform.localEulerAngles += new Vector3(90, 0, 0);
            Shot(rayCastPos.transform.forward);
        }
        
        towerRangeUI.rectTransform.localScale = towerRangeScale;
    }
    void Shot(Vector3 direction)
    {
        if (timer >= reloadTime) {
            if (type == TowerType.MachineGun || type == TowerType.RailGun)
            {
                RaycastHit hit;
                Ray ray = new Ray(rayCastPos.position, direction);
                Physics.Raycast(ray, out hit);
                Debug.DrawLine(ray.origin, hit.point, Color.green, 0.2f);

                if (type == TowerType.MachineGun)
                {
                    muzzlePos = (leftGun) ? guns[0] : guns[1];
                    var hi = Instantiate(muzzlePref, muzzlePos.position, muzzlePos.rotation, muzzlePos);
                    hi.GetComponent<MuzzleFlash>().lifeTime = 0.05f;
                    hi.GetComponent<MuzzleFlash>().scaleModifier = 0.02f;
                    leftGun = !leftGun;

                }
                else if(type == TowerType.RailGun)
                {
                    var hi = Instantiate(lazerPref, rayCastPos.position, rayCastPos.rotation, rayCastPos);
                    hi.GetComponent<Line>().destinatoin = hit.transform;
                }
                hit.transform.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);

            }
            timer = 0;
        }
    }
    void Kill(Transform obj)
    {
        if(target!=null)
            if(enemiesInRange.Contains(obj) || target == obj)
            {
                 target = null;
                 SortEnemies();
            }
        kills++;
        Debug.Log("Got kill by " + gameObject.name);
    }
    void SortEnemies()
    {
        if (enemiesInRange.Count > 0)
        {
            var j = 0;
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                var enemy = enemiesInRange[i];
                if (enemy.GetComponent<EnemyControl>().type == priority)
                {
                    var temp = enemiesInRange[j];
                    enemiesInRange[j] = enemy;
                    enemiesInRange[i] = temp;
                    j++;

                }
            }
        }   
    }
}
