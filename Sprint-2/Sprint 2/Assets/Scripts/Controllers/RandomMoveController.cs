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

			var randomDirection = DirectionManager.GetRandom();
			while (!Character.ActiveTile.Neighbours.ContainsKey(randomDirection))
				randomDirection = DirectionManager.GetRandom();

			Path = new List<BaseTile> { Character.ActiveTile.Neighbours[Direction.E] };
		}
	}

	protected void TryMoveEnemy()
	{
		if (Path.Count > 0)
		{
			Debug.Log($"MOVING to {Path[0].GridLocation2D} / ({Path.Count})");
			if (new TileSpecificMover().Move(Character, 1, Path[0]))
				Path.RemoveAt(0);
		}
	}
}
