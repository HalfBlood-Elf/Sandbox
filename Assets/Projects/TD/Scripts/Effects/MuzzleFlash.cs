using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : Pooler.PoolObject
{
	public Gradient gradient;

	public float maxLifeTime;
	public float scaleModifier;

	private Vector3 startLocalScale;
	private Material mat;
	private float time;

	void Awake()
	{
		mat = GetComponent<MeshRenderer>().material;
		startLocalScale = transform.localScale;
	}

	void Update()
	{
		time += Time.deltaTime;
		mat.SetColor("_TintColor", gradient.Evaluate(time / maxLifeTime));
		transform.localScale += new Vector3(scaleModifier, scaleModifier, scaleModifier * 5);
		if (time >= maxLifeTime)
		{
			Pool.ReturnToPool(this);
		}
	}

	public override void Spawned()
	{
		gameObject.SetActive(true);
		transform.rotation = transform.rotation * Quaternion.Euler(0, 0, Random.Range(0, 360));
		time = 0;
		transform.localScale = startLocalScale;
	}

	public override void Despawned()
	{
		gameObject.SetActive(false);
	}
}
