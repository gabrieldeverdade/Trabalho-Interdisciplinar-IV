using System;
using System.Collections;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
	public Attribute Health = new Attribute(50);

	public int JumpHeight = 1;

	public bool IsDamaging = false;

	public bool TakeHitAndIsAlive(int hitPower, bool hasInvunerableMoment)
	{
		if (hasInvunerableMoment && !IsDamaging)
			StartCoroutine(FlickOnScreen());
		
		if (Health.Remove(hitPower))
			return false;

		if (GetComponentInChildren<HealthManager>() != null)
			GetComponentInChildren<HealthManager>().ChangeBar(Health.Percentage);

		return true;
	}

	IEnumerator FlickOnScreen()
	{
		IsDamaging = true;
		var i = 4;
		var delay = new WaitForSeconds(0.25f);
		while (i > 0)
		{
			GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, i % 2 != 0 ? 0.3f : 1);
			i--;
			yield return delay;
		}
		GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);

		IsDamaging = false;
	}
}