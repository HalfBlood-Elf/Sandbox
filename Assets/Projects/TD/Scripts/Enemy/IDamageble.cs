using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IDamageble
{
	public void ApplyDamage(float dmg, Action<float> onDmgDone, Action<int> onKill);
}
