using UnityEngine;

public abstract class BaseTool : ConsumableObject
{
	readonly protected BaseResource[] HarverstableResources;

	public BaseTool(params BaseResource[] harverstableResources)
	{
		HarverstableResources = harverstableResources;
	}

	public override bool CanConsume() => CanUse();
	public abstract bool CanUse();
}