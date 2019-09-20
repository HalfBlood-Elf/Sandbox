using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class POV : MonoBehaviour {
    public GameObject towerPref;
    public Material towerCantBeBuilt;
    public Material towerCanBeBuilt;
    public Material towerNormal;
    public LayerMask layers;
    

    //private bool canBuild = false;
    private RaycastHit hit;
    private GameObject newTower;
    private GameObject selectedGO;
    void Start () {
		
	}
	
	void FixedUpdate () {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000, layers))
            {
                if(hit.transform.tag == "TowerSpawn")
                {
              
                        if(selectedGO == null)
                            selectedGO = hit.transform.gameObject;
                        else
                        {
                            selectedGO.SendMessage("Unselected");
                            selectedGO = hit.transform.gameObject;
                        }
                        hit.transform.SendMessage("Selected");
                }
                else
                {
                    if(selectedGO != null)
                    {
                        selectedGO.SendMessage("Unselected");
                        selectedGO = null;
                    }
                }
            }
        }


        #region
        /*
        if (Input.GetKeyDown(KeyCode.Q) && newTower == null)
        {
           newTower = Instantiate(towerPref);
        }
        if(newTower != null && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000, layers))
        {
            newTower.transform.position = hit.point;            

            if(canBuild)
            {
                newTower.GetComponent<TowerControl>().mat = towerCanBeBuilt;
            }
            else
            {
                newTower.GetComponent<TowerControl>().mat = towerCantBeBuilt;
            }

            if(Input.GetMouseButtonDown(0) && canBuild)
            {
                newTower.GetComponent<TowerControl>().mat = towerNormal;
                newTower = null;
            }
        }
        */
        #endregion
    }
}
