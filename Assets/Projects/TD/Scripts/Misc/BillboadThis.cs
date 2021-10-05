
using System.Collections.Generic;
using UnityEngine;

public class BillboadThis : MonoBehaviour {
	private void Update()
	{
		transform.LookAt(Camera.main.transform);
	}
}
