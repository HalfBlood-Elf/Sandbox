using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceRoller
{
	public class Shaker : MonoBehaviour
	{
		[SerializeField] private Transform container;
		[SerializeField] private float shakeForce = 20;
		[SerializeField] private int defaultSize = 5;
		[SerializeField] private int minSize = 3;


		private Vector3 v3zero = Vector3.zero;
		private IEnumerator routine;
		private Dice[] childs;
		private byte currentSize = 0;
		private void Start()
		{
			childs = new Dice[container.childCount];
			for (int i = 0; i < container.childCount; i++)
			{
				childs[i] = container.GetChild(i).GetComponent<Dice>();
			}
			SetSize((byte)defaultSize);
		}

		private void FixedUpdate()
		{
			foreach (var child in childs)
			{
				if (Vector3.SqrMagnitude(child.transform.position - container.position) > currentSize * currentSize)
				{
					child.transform.position = container.position;
				}
			}
		}

		public void Shake()
		{
			if (routine != null)
				StopCoroutine(routine);
			routine = Shaking();
			StartCoroutine(routine);
		}

		public void SetSize(byte size)
		{
			if (size < minSize) return;

			var childs = new Transform[container.childCount];
			for (int i = 0; i < container.childCount; i++)
			{
				childs[i] = container.GetChild(i);
			}
			foreach (var child in childs)
			{
				child.parent = null;
			}

			container.localScale = Vector3.one * size;

			foreach (var child in childs)
			{
				child.parent = container;
				child.position = container.position;
			}
			currentSize = size;
		}

		//private void Update()
		//{
		//	if (Input.GetMouseButtonDown(0))
		//	{
		//		Shake();
		//	}
		//}

		private IEnumerator Shaking()
		{
			if (container.childCount != childs.Length)
			{
				Start();
			}


			foreach (var dice in childs)
			{
				dice.rigidbody.AddForce(Random.insideUnitSphere * shakeForce, ForceMode.VelocityChange);
			}
			yield return null;
		}
	}
}