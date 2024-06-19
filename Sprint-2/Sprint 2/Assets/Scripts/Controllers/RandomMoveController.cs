using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomMoveController : CharacterBaseController<Enemy>
{
	[SerializeField]
	protected float CurrentTime = 0;

	protected float TimeToChange = 2;

	protected virtual void FixedUpdate()
	{
		CurrentTime += Time.deltaTime;

		FindNextRandomMove();
		TryMoveEnemy();
	}

	protected void FindNextRandomMove()
	{
		if (Character != null && Character.ActiveTile != null && CurrentTime > TimeToChange)
		{
			CurrentTime = 0;
			var amountOfTryies = 0;
			var randomDirection = DirectionManager.GetRandom();

			var walkableTiles = Character.ActiveTile.Neighbours.Where(c => c.Value.Walkable).ToDictionary(c => c.Key, c => c.Value);

			if (!walkableTiles.Any()) return;

			while (!walkableTiles.ContainsKey(randomDirection))
			{
				randomDirection = DirectionManager.GetRandom();
				amountOfTryies++;

				if (amountOfTryies > 10)
				{
					var walkableDirection = Character.ActiveTile.Neighbours.FirstOrDefault(n => n.Value.Walkable);

					if (walkableDirection.Equals(default(KeyValuePair<Direction, BaseTile>))) break;

					randomDirection = walkableDirection.Key;
					break;
				}
			}

			Path = new List<BaseTile> { Character.ActiveTile.Neighbours[randomDirection] };
		}
	}

	protected void TryMoveEnemy()
	{
		if (Path.Count > 0)
		{
			//Debug.Log($"MOVING to {Path[0].GridLocation2D} / ({Path.Count})");
			if (new TileSpecificMover().Move(Character, 1, Path[0]))
				Path.RemoveAt(0);
		}
	}
}
