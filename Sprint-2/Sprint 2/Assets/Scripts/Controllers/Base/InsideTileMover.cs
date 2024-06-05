using UnityEngine;
using UnityEngine.UI;

public class InsideTileMover
{
	Character Character;
	Position CurrentTileBounds;
	BaseTile NextTile;

	public bool Move(Vector2Int direction, Character character)
	{
		Character = character;
		if (Character.Position == null)
			Character.Position = Character.GetComponent<Rigidbody2D>().position;

		CurrentTileBounds = BuildBounds(Character.ActiveTile);

		var directionInfo = DirectionManager.GetDirection(direction.x, direction.y);
		
		if (directionInfo.Direction == Direction.None) return false;

		var nextPosition = character.Position + (directionInfo.Direction2D * 0.01f);
		Debug.Log(nextPosition);

		NextTile = GetNextTile(nextPosition);

		if (!NextTile.Walkable) return false;

		Character.UpdatePosition(nextPosition);

		Draw(character.ActiveTile);
		return false;
	}

	BaseTile GetNextTile(Vector3 position)
	{
		if (NextTile != null) NextTile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 0);

		var worldPosition = MapBuilder.Instance.Tilemap.WorldToCell(position);
		var worldPosition2D = new Vector2Int(worldPosition.x, worldPosition.y);
		var nextTile = MapManager.Instance.Map[worldPosition2D];
		nextTile.GetComponent<SpriteRenderer>().color = new Color(0,0,1,1);
		return nextTile;
	}

	public void Draw(BaseTile mapPosition, Color? color = null)
	{
		foreach (var neigbour in mapPosition.Neighbours)
		{
			if (neigbour.Value.GetComponentInChildren<Text>())
			{
				neigbour.Value.GetComponentInChildren<Text>().text = $"({neigbour.Value.transform.position.x},{neigbour.Value.transform.position.y.ToString("0.##")},{neigbour.Value.transform.position.z})";
				neigbour.Value.GetComponentInChildren<Text>().enabled = true;
			}

			neigbour.Value.GetComponent<SpriteRenderer>().color = new Color(neigbour.Value.Walkable ? 0 : 1, neigbour.Value.Walkable ? 1 : 0, 0, 1);
		}
	}

	public void DrawGizmos()
	{
		if(CurrentTileBounds != null)
		{
			Gizmos.color = Color.black;
			Gizmos.DrawLine(CurrentTileBounds.Left, CurrentTileBounds.Down);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(CurrentTileBounds.Down, CurrentTileBounds.Right);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(CurrentTileBounds.Right, CurrentTileBounds.Up);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(CurrentTileBounds.Up, CurrentTileBounds.Left);
			Gizmos.color = Color.gray;
		}

		if (CurrentTileBounds != null)
		{
			Gizmos.color = Color.gray;
			Gizmos.DrawSphere(Character.Position, 0.05f);
		}
	}

	Position BuildBounds(BaseTile tile)
		=> BuildBounds(tile.transform.position);

	Position BuildBounds(Vector3 position)
	{
		return new Position
		{
			Left = new Vector3(position.x - 0.5f, position.y, 0),
			Right = new Vector3(position.x + 0.5f, position.y, 0),
			Up = new Vector3(position.x, position.y + 0.25f, 0),
			Down = new Vector3(position.x, position.y - 0.25f, 0),
		};
	}
}