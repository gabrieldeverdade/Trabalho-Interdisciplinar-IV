using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseInventory : MonoBehaviour
{
	public List<ResourceInBag> ResourcesInBag = new List<ResourceInBag>();

	public void AddResource(Resource resource, int amount)
	{
		var resourceInBag = ResourcesInBag.FirstOrDefault(c => c.Resource == resource);
		if (resourceInBag == null)
			ResourcesInBag.Add(new ResourceInBag { Resource = resource, Amount = amount });
		else
			resourceInBag.Amount += amount;
	}

	public void ConsumeResource(Resource resource, int amount)
	{
		var resourceInBag = ResourcesInBag.FirstOrDefault(c => c.Resource == resource);
		if (resourceInBag != null)
			resourceInBag.Amount -= amount;
	}
}

[Serializable]
public class ResourceInBag
{
	public Resource Resource;
	public int Amount;
}