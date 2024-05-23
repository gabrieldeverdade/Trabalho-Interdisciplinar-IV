using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static ArrowTranslator;

public abstract class BaseTile : MonoBehaviour
{
	public float G;
	public float H;
	public float F => G + H;

	public Dictionary<Direction, BaseTile> Neighbours = new();

	public bool ShowPathAmount = true;
	public bool Walkable = false;
	public bool IsBlocked => /*!CanUsePath &&*/ !Walkable;
	public bool Resourceable;
	public BaseTile Previous;
	public Resource Resource;
	public int TotalAmount = 3;

	public bool Consumable => Resource.Amount > 0;
	public int UsingPath => MapManager.Instance.Paths.Count(c => c.Value.Contains(this));
	public bool CanUsePath => UsingPath < TotalAmount;

	public Vector3Int GridLocation;
	public Vector2Int GridLocation2D => new Vector2Int(GridLocation.x, GridLocation.y);

	[SerializeField] List<Sprite> Directions = new List<Sprite>();

	public void ShowTile()
	{
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0.7f);
		gameObject.GetComponent<SpriteRenderer>().sprite = MapBuilder.Instance.Tiles[0].ValidTiles[1].sprite;
	}

	public void HideTile()
	{
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		gameObject.GetComponent<SpriteRenderer>().sprite = MapBuilder.Instance.Tiles[0].ValidTiles[0].sprite;
		SetText("None");
	}

	public void SetText(string text)
	{
		if(gameObject.GetComponentInChildren<Text>() != null && gameObject.GetComponentInChildren<Text>().enabled)
			gameObject.GetComponentInChildren<Text>().text = text;
	}

	public void SetArrowSprite(ArrowDirection d)
	{
		var arrow = GetComponentsInChildren<SpriteRenderer>()[1];
		if (d == ArrowDirection.None)
			arrow.color = new Color(1, 1, 1, 0);
		else
		{
			arrow.color = new Color(1, 1, 1, 1);
			arrow.sprite = Directions[(int)d];
			arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
		}

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
		if (map.ContainsKey(locationToCheck) && Mathf.Abs(map[locationToCheck].GridLocation.z - z) < MapManager.Instance.MapJumpHeight)
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
}
