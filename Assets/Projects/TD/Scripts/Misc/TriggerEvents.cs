using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TriggerEvents : MonoBehaviour
{
	[SerializeField] private bool log = true;

	public event System.Action<Collider> OnTriggerEnterFired;
	public event System.Action<Collider> OnTriggerExitFired;
	public Collider Trigger { 
		get 
		{
			if (trigger == null)
			{
				trigger = GetComponent<Collider>();

			}
			trigger.isTrigger = true;
			return trigger;
		}

		protected set => trigger = value;
	}

	private Collider trigger;

	protected virtual void OnTriggerEnter(Collider other)
	{
		if (log) Debug.Log($"{other.name} entered {name} triger");
		OnTriggerEnterFired?.Invoke(other);
	}


	protected virtual void OnTriggerExit(Collider other)
	{
		if (log) Debug.Log($"{other.name} exited {name} triger");
		OnTriggerExitFired?.Invoke(other);
	}
}
