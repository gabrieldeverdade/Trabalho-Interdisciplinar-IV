using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBuilder : SingletonMonoBehaviour<MapBuilder>
{
	public Tilemap Tilemap;
	public BaseTile OverlayPrefab;
	public List<Tile> UnwalkableTiles;
	public List<Tile> ClimbableTiles;
	public List<Tile> WorkBanches;
	public List<Tile> UnflayableTiles;

	public List<Resource> ResourceableTiles;

	private void Start()
	{
		var grid = GetComponentInChildren<Grid>();
		Tilemap = grid.GetComponentInChildren<Tilemap>();
		MapManager.Instance.Map = new Dictionary<Vector2Int, BaseTile>();
		BuildTilemapContainer(Tilemap);
	}

	void BuildTilemapContainer(Tilemap tileMap)
	{
		BoundsInt bounds = tileMap.cellBounds;

		for (int z = bounds.max.z; z >= bounds.min.z; z--)
			for (int y = bounds.min.y; y < bounds.max.y; y++)
				for (int x = bounds.min.x; x < bounds.max.x; x++)
					BuildTiles(x, y, z);

		UpdateNeighbours();
	}

	void BuildTiles(int x, int y, int z)
	{
		var tileLocation = new Vector3Int(x, y, z);
		var tileKey = new Vector2Int(x, y);

		if (Tilemap.HasTile(tileLocation) && !MapManager.Instance.Map.ContainsKey(tileKey))
		{
			var overlayTile = Instantiate(OverlayPrefab, Tilemap.transform);
			UpdateOverlayStatus(overlayTile, tileLocation, z);
			MapManager.Instance.Map.Add(tileKey, overlayTile);
		}
	}

	void UpdateOverlayStatus(BaseTile overlayTile, Vector3Int tilemapLocation, int tileHeight) 
	{
		var tile = Tilemap.GetTile(tilemapLocation);
		var cellWorldPosition = Tilemap.GetCellCenterWorld(tilemapLocation);

		overlayTile.Height = tileHeight;

		overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y + 0.0001f, cellWorldPosition.z + 1);
		overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.0f);
		overlayTile.GetComponent<SpriteRenderer>().sortingOrder = Tilemap.GetComponent<TilemapRenderer>().sortingOrder + (int)cellWorldPosition.z;
		overlayTile.GridLocation = tilemapLocation;

		overlayTile.WorldPosition = new TileBounds
		{
			Left = new Vector3(overlayTile.transform.position.x - 0.5f, overlayTile.transform.position.y, 0),
			Right = new Vector3(overlayTile.transform.position.x + 0.5f, overlayTile.transform.position.y, 0),
			Up = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y + 0.25f, 0),
			Down = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y - 0.25f, 0),
		};

		var foundResourceTile = ResourceableTiles.Find(c => c.Tile == tile);
		if (foundResourceTile != null)
		{
			overlayTile.Resourceable = true;
			overlayTile.Resource = foundResourceTile;
		}
		else
			overlayTile.Resourceable = false;


		overlayTile.WorkBench = WorkBanches.Find(c => c == tile);
		overlayTile.Flyable = !UnflayableTiles.Find(c => c == tile);
		overlayTile.Walkable = !UnwalkableTiles.Find(c => c == tile);
		overlayTile.Climbable = ClimbableTiles.Find(c => c == tile);
	}

	void UpdateNeighbours()
	{
		foreach (var map in MapManager.Instance.Map.Values)
			map.GetNeightbourTiles(new List<BaseTile>());
	}
}