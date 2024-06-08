using UnityEngine;

public class RandomMover
{
	public float DecisionClock = 11;
	public float ChangeDecisionAt = 10;
	public float Speed = 10;
	public System.Random RNG;

	public void Move(Character character)
	{
		var characterRigidBody = character.GetComponent<Rigidbody2D>();
		characterRigidBody.velocity = new Vector2(RNG.Next(-1, 2), RNG.Next(-1, 2)) * Speed;

		DecisionClock += Time.deltaTime;
		if(DecisionClock > ChangeDecisionAt)
		{
			DecisionClock = 0;
			characterRigidBody.velocity = new Vector2(RNG.Next(-1, 2), RNG.Next(-1, 2)) * Speed;
		}
	}
}