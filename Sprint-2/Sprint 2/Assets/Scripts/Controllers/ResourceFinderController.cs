using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceFinderController : RandomMoveController
{
	[SerializeField] bool HasFindResource = false;
	[SerializeField] BaseTile FocusedResource;

	bool GoingToResource = false;
	bool GoingToSpawn = false;

	protected override void FixedUpdate()
	{
		if (Character.ActiveTile == null) return;

		CurrentTime += Time.deltaTime;

		if (!HasFindResource)
		{
			base.FixedUpdate();
			HasFoundResource();
		}
		else if (Path.Count == 0)
		{
			//Debug.Log($"VERIFYING PROXIMITY: {Path.Count}");
			VerifyProximityToResource();
			VerifyProximityToSpawn();
		}

		TryMoveEnemy();
	}

	void HasFoundResource()
	{
		if (Character.ActiveTile == null) return;

		if (FocusedResource != null) return;

		var tiles = new RangeFinder().GetTilesInRange(Character.ActiveTile, 1);
		var resourceTile = tiles.FirstOrDefault(t => t.Resourceable);

		if (resourceTile != null)
		{
			//Debug.Log("FOUND RESOURCE");
			FocusedResource = resourceTile;
			Path = new PathFinder().Find(Character, Character.ActiveTile, FocusedResource, 0);
			//Debug.Log($"FOUND PATH: {Path.Count}");
			GoingToResource = true;
			HasFindResource = true;
		}
	}

	void VerifyProximityToResource()
	{
		var tiles = new RangeFinder().GetTilesInRange(Character.ActiveTile, 1);
		if (GoingToResource && tiles.Any(t => t == FocusedResource))
		{
			GoingToSpawn = true;
			GoingToResource = false;

			//Debug.Log("GOING TO RESOURCE");
			var enemy = Character.GetComponent<Enemy>();
			enemy.GotResource(FocusedResource.Resource, 10);
			Path = new PathFinder().Find(Character, Character.ActiveTile, enemy.Spawn.SpawnTile, 0);
		}
	}

	void VerifyProximityToSpawn()
	{
		var tiles = new RangeFinder().GetTilesInRange(Character.ActiveTile, 1);
		var enemy = Character.GetComponent<Enemy>();
		if (GoingToSpawn && tiles.Any(t => t == enemy.Spawn.SpawnTile))
		{
			GoingToSpawn = false;
			GoingToResource = true;

			//Debug.Log("GOING TO SPAWN");
			var resource = enemy.DropResource();

			enemy.Spawn.GetComponent<BaseInventory>().AddResource(resource.Resource, resource.Amount);
			enemy.DropResource();

			var walkableNeighbour = FocusedResource.Neighbours.Values.FirstOrDefault(c => c.Walkable);

			if (walkableNeighbour.Equals(default(KeyValuePair<Direction, BaseTile>))) return;

			var resourceTile = Character.CanFly ? FocusedResource : walkableNeighbour;

			Path = new PathFinder().Find(Character, Character.ActiveTile, resourceTile, 0);
		}
	}

}
