
using System.Linq;
using UnityEngine;

public class CharacterStalkController : RandomMoveController
{
	public Character Destination;
	bool FoundCharacter = false;

	protected override void FixedUpdate()
	{
		InitializeActiveTile();

		if (!FoundCharacter)
		{
			base.FixedUpdate();
			TryToFindCharacter();
		}
		else
		{ 
			TryMoveEnemy();
			FindNewPath();
		}
	}

	void TryToFindCharacter()
	{
		if (new RangeFinder().GetTilesInRange(Character.ActiveTile, 3).Any(c => Destination.ActiveTile == c))
			FoundCharacter = true;
	}
		
	void FindNewPath() { if (Character != null && Character.ActiveTile != null) FindPathBetween(Character, Destination); }

	public void SetDestination(Character character) => Destination = character;
}
