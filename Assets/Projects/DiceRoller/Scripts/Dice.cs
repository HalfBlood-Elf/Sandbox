using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace DiceRoller
{

	[RequireComponent(typeof(Rigidbody))]
	public class Dice : MonoBehaviour
	{
		public bool isMoving;
		

		[System.Serializable]
		public class Side
		{
			public byte value;
			public Transform sideObj;
		}

		[SerializeField] private Side[] sides;

		[SerializeField] private Material normalMat;
		[SerializeField] private Material movingMat;
		[SerializeField] private byte currPoints;
		public new Rigidbody rigidbody;
		private new MeshRenderer renderer;
		private void Start()
		{
			if (rigidbody == null)
				rigidbody = GetComponent<Rigidbody>();
			if (renderer == null)
				renderer = GetComponent<MeshRenderer>();
		}

		private void Update()
		{
			isMoving = rigidbody.linearVelocity.sqrMagnitude >= 0.0001 || rigidbody.angularVelocity.sqrMagnitude >= 0.0001;
			if(normalMat != null && movingMat != null)
			{
				renderer.material = (isMoving) ? movingMat : normalMat;
			}
			currPoints = GetCurrentScores();
		}

		public byte GetCurrentScores()
		{
			var a = float.MaxValue;
			var minI = 0;
			for (int i = 0; i < sides.Length; i++)
			{
				var currSideDirr = sides[i].sideObj.transform.up;
				var angle = Vector3.Angle(Vector3.up, currSideDirr);
				if (angle < a)
				{
					a = angle;
					minI = i;
				}
			}

			return sides[minI].value;
		}

	}
}
