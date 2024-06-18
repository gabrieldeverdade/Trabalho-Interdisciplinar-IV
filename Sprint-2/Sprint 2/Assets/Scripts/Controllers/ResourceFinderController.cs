using System.Linq;
using UnityEngine;

public class ResourceFinderController : RandomMoveController
{
	[SerializeField] bool HasFindResource = false;
	BaseTile FocusedResource;

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
			Debug.Log($"VERIFYING PROXIMITY: {Path.Count}");
			VerifyProximityToResource();
			VerifyProximityToSpawn();
		}

		TryMoveEnemy();
	}

	void HasFoundResource()
	{
		if (Character.ActiveTile == null) return;

		if (FocusedResource != null) return;

		var tiles = new RangeFinder().GetTilesInRange(Character.ActiveTile, 3);
		var resourceTile = tiles.FirstOrDefault(t => t.Resourceable);

		if (resourceTile != null)
		{
			Debug.Log("FOUND RESOURCE");
			FocusedResource = resourceTile;
			Path = new PathFinder().Find(Character, Character.ActiveTile, FocusedResource, 0);
			Debug.Log($"FOUND PATH: {Path.Count}");
			HasFindResource = true;
		}
	}

	void VerifyProximityToResource()
	{
		var tiles = new RangeFinder().GetTilesInRange(Character.ActiveTile, 1);
		if (tiles.Any(t => t == FocusedResource))
		{
			Debug.Log("GOING TO RESOURCE");
			var enemy = Character.GetComponent<Enemy>();
			enemy.GotResource(FocusedResource.Resource, 10);
			Path = new PathFinder().Find(Character, Character.ActiveTile, enemy.Spawn.SpawnTile, 0);
		}
	}

	void VerifyProximityToSpawn()
	{
		var tiles = new RangeFinder().GetTilesInRange(Character.ActiveTile, 1);
		var enemy = Character.GetComponent<Enemy>();
		if (tiles.Any(t => t == enemy.Spawn.SpawnTile))
		{
			enemy.Spawn.GetComponent<BaseInventory>().AddResource(enemy.CurrentResource, enemy.ResourceAmount);
			enemy.DropResource();

			Path = new PathFinder().Find(Character, Character.ActiveTile, FocusedResource, 0);
		}
	}

}
