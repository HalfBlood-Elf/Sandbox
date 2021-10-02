using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class NavMeshAgentPathDebug : MonoBehaviour
{
	[SerializeField] private NavMeshAgent agent;
	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		if (agent.hasPath)
		{
			lineRenderer.positionCount = agent.path.corners.Length;
			lineRenderer.SetPositions(agent.path.corners);
			lineRenderer.enabled = true;
		}
		else
		{
			lineRenderer.enabled = false;

		}
	}
} 
