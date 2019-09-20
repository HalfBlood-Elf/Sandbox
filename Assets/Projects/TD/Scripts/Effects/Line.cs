using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour {
    LineRenderer lineRenderer;
    public AnimationCurve curve;
    public Transform destinatoin;
    private float timer;
    public float lifeTime;
    //private float startWidth;

	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        //startWidth = lineRenderer.startWidth;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, destinatoin.position);
        lineRenderer.widthMultiplier = curve.Evaluate(timer/lifeTime);

        if (timer > lifeTime)
            Destroy(gameObject);
    }
}
