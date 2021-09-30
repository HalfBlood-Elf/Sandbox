using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateMuzzleFlash : MonoBehaviour {
    public GameObject pref;
    public Transform transf;

	void Start() {

    }
	
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            Instantiate(pref, transf.position, transf.rotation);
        }
	}
}
