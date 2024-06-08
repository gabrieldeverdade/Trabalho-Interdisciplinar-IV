using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseTile : MonoBehaviour
{
	public float G;
	public float H;
	public float F => G + H;

	public Dictionary<Direction, BaseTile> Neighbours = new();

	public bool ShowPathAmount = true;
	public bool Walkable = false;
	public bool Climbable = false;
	public bool Resourceable = false;
	public bool WorkBench = false;

	public int CurrentResource;
	public int Height = 0;
	public bool IsBlocked => /*!CanUsePath &&*/ !Walkable;
	public BaseTile Previous;
	public Resource Resource;
	public int TotalAmount = 3;

	public bool Consumable => Resource.Amount > 0;
	public int UsingPath => MapManager.Instance.Paths.Count(c => c.Value.Contains(this));
	public bool CanUsePath => UsingPath < TotalAmount;

	public Vector3Int GridLocation;
	public Vector2Int GridLocation2D => new Vector2Int(GridLocation.x, GridLocation.y);
	
	public TileBounds WorldPosition;



	[SerializeField] List<Sprite> Directions = new List<Sprite>();

	public void ShowTile()
	{
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.7f);
	}

	public void HideTile()
	{
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		SetText("None");
	}

	public void SetText(string text)
	{
		if (gameObject.GetComponentInChildren<Text>() != null)
		{
			gameObject.GetComponentInChildren<Text>().enabled = true;
			gameObject.GetComponentInChildren<Text>().text = text;
		}
	}

	public void HideText()
	{
		gameObject.GetComponentInChildren<Text>().enabled = false;
	}

	public List<BaseTile> GetNeightbourTiles(List<BaseTile> searchableTiles)
	{
		var map = searchableTiles.Count == 0 ? MapManager.Instance.Map : searchableTiles.ToDictionary(c => c.GridLocation2D, c => c);
		var neighbours = new List<BaseTile>();
		BaseTile found = null;

		if (HasNorthNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.N, found);
		}
		if (HasNorthEastNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.NE, found);
		}
		if (HasNorthWestNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.NW, found);
		}

		if (HasSouthNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.S, found);
		}
		if (HasSouthEastNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.SE, found);
		}

		if (HasSouthWestNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.SW, found);
		}

		if (HasEastNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.E, found);
		}
		if (HasWestNeighbour(map, out found))
		{
			neighbours.Add(found);
			Neighbours.Add(Direction.W, found);
		}

		return neighbours;
	}

	public bool HasNorthNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, North, out found);
	public bool HasNorthEastNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, North+East, out found);
	public bool HasNorthWestNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, North+West, out found);

	public bool HasSouthNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, South, out found);
	public bool HasSouthEastNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, South+East, out found);
	public bool HasSouthWestNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, South+West, out found);

	public bool HasEastNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, East, out found);
	public bool HasWestNeighbour(Dictionary<Vector2Int, BaseTile> map, out BaseTile found) => HasNeighbour(map, West, out found);

	bool HasNeighbour(Dictionary<Vector2Int, BaseTile> map, Vector2Int direction, out BaseTile foundTile)
	{
		foundTile = null;
		var point = GridLocation;
		var (x, y, z) = (point.x, point.y, point.z);

		var locationToCheck = new Vector2Int(x, y) + direction;
		if (map.ContainsKey(locationToCheck))
		{
			foundTile = map[locationToCheck];
			return true;
		}
		return false;
	}

	public Vector2Int North = new Vector2Int(0, 1);
	public Vector2Int NorthEast => North + East;
	public Vector2Int NorthWest => North + West;

	public Vector2Int South = new Vector2Int(0, -1);
	public Vector2Int SouthEast => South + East;
	public Vector2Int SouthWest => South + West;

	public Vector2Int East = new Vector2Int(1, 0);
	public Vector2Int West = new Vector2Int(-1, 0);

	public TileBounds BuildBounds()
		=> BuildBounds(transform.position);

	TileBounds BuildBounds(Vector3 position)
	{
		return new TileBounds
		{
			Left = new Vector3(position.x - 0.5f, position.y, 0),
			Right = new Vector3(position.x + 0.5f, position.y, 0),
			Up = new Vector3(position.x, position.y + 0.25f, 0),
			Down = new Vector3(position.x, position.y - 0.25f, 0),
		};
	}
}
