using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSettings", menuName = "ScriptableObjects/ProjectileSettings", order = 1)]
public class ProjectileSettings: ScriptableObject
{
	public float onHitDmg = 20;
	public int maxHits = 1;
	public float consecutiveHitsDmgMultiplier = 1;
	public float speed;
	public float lifeTime = 50;
	// 3d model
	//ondespawn (effect)
	
}
