using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSpawn : MonoBehaviour {
    public GameObject towerPref;
    public Text text;


    private bool selected;
    private Vector3 normalScale;
    
	// Use this for initialization
	void Start () {
        normalScale = transform.localScale;
        text = GameObject.Find("TowerSpawn text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.localScale =(selected) ? normalScale * 1.1f : normalScale;
        
	}
    void SpawnTower()
    {
    }
    void Selected()
    {
        selected = true;
        text.transform.parent.gameObject.SetActive(true);
        text.text = @"Press 1 to set new tower.";

    }
    void Unselected()
    {
        selected = false;
        text.transform.parent.gameObject.SetActive(false);
        text.text = @"";
    }
}
