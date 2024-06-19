using System;
using UnityEngine;

[Serializable]
public class ResourceStats
{
	public Resource Resource;
	public int Amount;

	public ResourceStats(Resource resource, int amount)
	{
		Resource = resource;
		Amount = amount;
	}

	public void SetResource(Resource resource, int amount)
	{
		Resource = resource;
		Amount = amount;
	}

	public bool ConsumeResource(Resource resource, int amount)
	{
		if (Resource != resource || Amount - amount < 0)
			return false;

		Amount -= amount;
		return true;
	}
}

public class Enemy : Character
{
	int Range = 3;
	public int Points { get; private set; } = 0;
	public bool IsAttacker { get; private set; }
	Attribute LifeSpan = new Attribute(90);
	ResourceStats CurrentResource = null;
	
	public SpawnArea Spawn { get; private set; }

	[SerializeField] ResourceStats ResourceToWalk;

	public void FixedUpdate()
	{
		if (LifeSpan.Remove(Time.deltaTime))
			Destroy(gameObject);
	}

	public bool CanWalk()
		=> ResourceToWalk.ConsumeResource(ResourceToWalk.Resource, 10);

	public override void TakeHit(Character character)
		=> base.TakeHit(character);

	public void HitCharacter()
		=> AttakedPlayer();

	public void GotResource(Resource carryResource, int amount)
	{
		CurrentResource = new ResourceStats(carryResource, amount);
	}

	public ResourceStats DropResource()
	{
		CollectedResource();
		var resourceToDrop = CurrentResource;
		CurrentResource = null;
		return resourceToDrop;
	}

	void AttakedPlayer() { if (IsAttacker) Points++; else Points--; }
	void CollectedResource() { if (IsAttacker) Points--; else Points++; }

	public void SetJob(SpawnArea spawn,bool isAttacker)
	{
		IsAttacker = isAttacker;
		Spawn = spawn;
		Points = 0;
	}
}
