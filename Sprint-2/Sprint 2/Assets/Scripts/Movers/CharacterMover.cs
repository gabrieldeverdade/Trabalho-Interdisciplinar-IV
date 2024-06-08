using UnityEngine;

public class CharacterMover
{
	public bool Move(Vector2Int direction, Character character)
	{
		var directionInfo = DirectionManager.GetDirection(direction.x, direction.y);
		
		if (directionInfo.Direction == Direction.None)
			return false;

		var nextPosition = character.Position + (directionInfo.Direction2D * 0.02f);

		var nextTile = MapManager.Instance.GetCellFromWorldPosition(nextPosition);

		if (TileIsBlocked(character, nextTile))
			return false;

		character.UpdatePosition(nextPosition);

		return false;
	}

	bool TileIsBlocked(Character character, BaseTile tile)
		=> tile == null || !tile.Walkable || character.CanReachHeight(tile);
}