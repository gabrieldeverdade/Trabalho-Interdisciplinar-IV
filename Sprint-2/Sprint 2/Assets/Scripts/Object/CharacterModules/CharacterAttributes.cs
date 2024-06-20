using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[Serializable]
public class CharacterAttributes : MonoBehaviour
{
	[SerializeField] public Attribute Health = new Attribute(50);
	[SerializeField] public Attribute Stamina = new Attribute(200);
	[SerializeField] public bool IsDamaging = false;

	public int JumpHeight = 1;

	public bool TakeHitAndIsAlive(int hitPower, bool hasInvunerableMoment)
	{
		if (!IsDamaging)
		{
			if(GetComponentInParent<PlayerAnimation>() != null)
			{
				var newPos = DirectionManager.GetDirection(DirectionManager.GetInverse(GetComponentInParent<PlayerAnimation>().CurrentDirection));
				var rigid = GetComponent<Rigidbody2D>().transform.position ;
				GetComponentInParent<Character>().UpdatePosition(rigid + ((Vector3)newPos * 0.5f));
			}

			if (Health.Remove(hitPower))
				return false;

			if(hasInvunerableMoment)
				StartCoroutine(FlickOnScreen());

			if (GetComponentsInChildren<HealthManager>().ElementAtOrDefault(0) != null)
				GetComponentsInChildren<HealthManager>()[0].ChangeBar(Health.Percentage);
		}
		return true;
	}


	public void AddHealth(int amount)
	{
		if (GetComponentsInChildren<HealthManager>().ElementAtOrDefault(0) != null)
			GetComponentsInChildren<HealthManager>()[0].ChangeBar(Health.Percentage);
		
		Health.Add(amount);
	}
	public void AddStamina(int amount)
	{
		UpdateStamina();
		Stamina.Add(amount);
	}

	public bool ConsumeStamina(int amount)
	{
		var canConsumeStamina = Stamina.Add(-amount);

		UpdateStamina();

		return canConsumeStamina;
	}

	public void UpdateStamina()
	{

		if (GetComponentsInChildren<HealthManager>().ElementAtOrDefault(1) != null)
			GetComponentsInChildren<HealthManager>()[1].ChangeBar(Stamina.Percentage);
	}

	IEnumerator FlickOnScreen()
	{
		IsDamaging = true;
		var i = 4;
		var delay = new WaitForSeconds(0.25f);
		while (i > 0)
		{
			GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, i % 2 == 0 ? 0.3f : 1);
			i--;
			yield return delay;
		}
		GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);

		IsDamaging = false;
	}
}