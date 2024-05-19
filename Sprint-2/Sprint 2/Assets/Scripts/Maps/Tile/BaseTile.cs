using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static ArrowTranslator;

public abstract class BaseTile : MonoBehaviour
{
	public int G;
	public int H;
	public int F => G + H;

	public bool ShowPathAmount = true;
	public bool IsBlocked;
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
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		//if (ShowPathAmount)
		//{
		//	GetComponentInChildren<TextMeshPro>().enabled = false;
		//	GetComponentInChildren<TextMeshPro>().color = new Color(0, 0, 0, 1);
		//	GetComponentInChildren<TextMeshPro>().text = $"{UsingPath}/{TotalAmount}";
		//}
	}

	public void HideTile()
	{
		gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
		//if (ShowPathAmount)
		//{
		//	GetComponentInChildren<TextMeshPro>().enabled = true;
		//	GetComponentInChildren<TextMeshPro>().color = new Color(0, 0, 0, 1);
		//	GetComponentInChildren<TextMeshPro>().text = $"0/{TotalAmount}";
		//	SetArrowSprite(ArrowDirection.None);
		//}
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
			neighbours.Add(found);
		if (HasNorthEastNeighbour(map, out found))
			neighbours.Add(found);
		if (HasNorthWestNeighbour(map, out found))
			neighbours.Add(found);

		if (HasSouthNeighbour(map, out found))
			neighbours.Add(found);
		if (HasSouthEastNeighbour(map, out found))
			neighbours.Add(found);
		if (HasSouthWestNeighbour(map, out found))
			neighbours.Add(found);

		if (HasEastNeighbour(map, out found))
			neighbours.Add(found);
		if (HasWestNeighbour(map, out found))
			neighbours.Add(found);

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
