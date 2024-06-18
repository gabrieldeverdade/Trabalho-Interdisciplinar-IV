using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class CharacterAttributes : MonoBehaviour
{
	[SerializeField] public Attribute Health = new Attribute(50);
	[SerializeField] public bool IsDamaging = false;

	public int JumpHeight = 1;

	public bool TakeHitAndIsAlive(int hitPower, bool hasInvunerableMoment, Character character)
	{
		if (hasInvunerableMoment && !IsDamaging)
		{
			var characterDirection = DirectionManager.GetDirection(DirectionManager.GetInverse(
				GetComponentInParent<PlayerAnimation>() == null
					? character.GetComponent<PlayerAnimation>().CurrentDirection
					: GetComponentInParent<PlayerAnimation>().CurrentDirection)
				);

			var newPos = DirectionManager.GetDirection(DirectionManager.GetInverse(GetComponentInParent<PlayerAnimation>().CurrentDirection));
			var rigid = GetComponent<Rigidbody2D>().transform.position ;
			GetComponentInParent<Character>().UpdatePosition(rigid + ((Vector3)newPos * 0.5f));

			if (Health.Remove(hitPower))
				return false;

			StartCoroutine(FlickOnScreen());

		}

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
			GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, i % 2 == 0 ? 0.3f : 1);
			i--;
			yield return delay;
		}
		GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);

		IsDamaging = false;
	}
}