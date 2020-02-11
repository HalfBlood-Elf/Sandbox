using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DiceRoller
{

	[RequireComponent(typeof(Rigidbody))]
	public class Dice : MonoBehaviour
	{
		[System.Serializable]
		public class Side
		{
			public Vector3 direction;
			public byte value;
			public Transform sideObj;
		}

		[SerializeField] private Side[] sides;

		public new Rigidbody rigidbody;
		private void Start()
		{
			if (rigidbody == null)
				rigidbody = GetComponent<Rigidbody>();
		}


	}
}
