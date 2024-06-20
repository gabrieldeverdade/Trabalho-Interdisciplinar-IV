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
	Attribute LifeSpan = new Attribute(120);
	ResourceStats CurrentResource = null;

	[SerializeField] int CurrentPoints => Points;

	public float[] Members;
	[SerializeField] public bool IsAttacker { get; private set; }
	[SerializeField] public bool CanChangeToAttacker { get; private set; }
	
	public SpawnArea Spawn { get; private set; }

	[SerializeField] ResourceStats ResourceToWalk;

	public void FixedUpdate()
	{
		if (LifeSpan.Remove(Time.deltaTime))
			Destroy(gameObject);
	}

	public bool CanWalk()
		=> ResourceToWalk.ConsumeResource(ResourceToWalk.Resource, 10);

	public override void TakeHit(Character character, int damage)
		=> base.TakeHit(character, damage);

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

	public void SetJob(SpawnArea spawn, float[] members)
	{
		Members = members;
		IsAttacker = new UnityRandomGenerator().GenerateDouble() > members[0];
		CanChangeToAttacker = new UnityRandomGenerator().GenerateDouble() > members[1];
		Spawn = spawn;
		Points = 0;
	}
}
