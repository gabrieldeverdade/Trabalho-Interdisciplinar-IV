using UnityEngine;

public class Enemy : Character
{
	int TimeToLive = 90;
	float CurrentLife = 0;
	public int Points = 0;
	public bool IsAttacker = false;

	public Resource CurrentResource;
	public int ResourceAmount;

	public SpawnArea Spawn;

	public override void TakeHit(Character character)
	{
		base.TakeHit(character);
	}

	public void FixedUpdate()
	{
		CurrentLife += Time.deltaTime;

		if(CurrentLife > TimeToLive)
			Destroy(gameObject);
	}

	public void GotResource(Resource carryResource, int amount)
	{
		CurrentResource = carryResource;
		ResourceAmount += amount;
	}

	public void DropResource()
	{
		CurrentResource = null;
		ResourceAmount = 0;
		Points++;
	}

	public void AttakedPlayer() => Points++;

	public void SetJob(bool isAttacker)
	{
		IsAttacker = isAttacker;
	}
}
