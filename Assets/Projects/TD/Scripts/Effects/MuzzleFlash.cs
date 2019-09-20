using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {
    public Gradient gradient;

    float time;
    public float lifeTime;
    public float scaleModifier;

	void Awake () {
        transform.rotation = transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360));
	}
	
	void FixedUpdate () {
        time += Time.deltaTime;
        GetComponent<MeshRenderer>().material.SetColor("_TintColor", gradient.Evaluate(time / lifeTime));
        transform.localScale += new Vector3(scaleModifier,scaleModifier,scaleModifier *5);
        if(time >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
