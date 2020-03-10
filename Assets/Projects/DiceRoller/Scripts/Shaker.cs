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
		private Dice[] dices;
		private byte currentSize = 0;
		private void Start()
		{
			dices = new Dice[container.childCount];
			for (int i = 0; i < container.childCount; i++)
			{
				dices[i] = container.GetChild(i).GetComponent<Dice>();
			}
			SetSize((byte)defaultSize);
		}

		private void FixedUpdate()
		{
			foreach (var child in dices)
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
		private void OnStopShake()
		{
			Debug.Log("Stopped");
			foreach (var dice in dices)
			{
				Debug.Log("score: "+ dice.GetCurrentScores());
			}
		}
		private IEnumerator Shaking()
		{
			if (container.childCount != dices.Length)
			{
				Start();
			}


			foreach (var dice in dices)
			{
				dice.rigidbody.AddForce(Random.insideUnitSphere * shakeForce, ForceMode.VelocityChange);
			}

			yield return null;

			bool allStopped = false;
			while (!allStopped)
			{
				foreach (var dice in dices)
				{
					if (dice.isMoving)
					{
						allStopped = false;
						break;
					}
					allStopped = true;
				}
				yield return null;
			}

			OnStopShake();
		}
	}
}