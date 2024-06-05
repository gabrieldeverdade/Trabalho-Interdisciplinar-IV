using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBuilder : SingletonMonoBehaviour<MapBuilder>
{
	public Tilemap Tilemap;
	public BaseTile OverlayPrefab;
	public List<Tile> UnwalkableTiles;
	public List<Tile> ClimbableTiles;

	private void Start()
	{
		var grid = GetComponentInChildren<Grid>();
		Tilemap = grid.GetComponentInChildren<Tilemap>();
		BuildTilemapContainer(Tilemap);
	}

	void BuildTilemapContainer(Tilemap tileMap)
	{
		MapManager.Instance.Map = new Dictionary<Vector2Int, BaseTile>();

		BoundsInt bounds = tileMap.cellBounds;

		for (int z = bounds.max.z; z >= bounds.min.z; z--)
		{
			for (int y = bounds.min.y; y < bounds.max.y; y++)
			{
				for (int x = bounds.min.x; x < bounds.max.x; x++)
				{
					var tileLocation = new Vector3Int(x, y, z);
					var tileKey = new Vector2Int(x, y);

					if (tileMap.HasTile(tileLocation) && !MapManager.Instance.Map.ContainsKey(tileKey))
					{
						var tile = tileMap.GetTile(tileLocation);
						var overlayTile = Instantiate(OverlayPrefab, tileMap.transform);
						var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

						overlayTile.Height = z;

						overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y+0.0001f, cellWorldPosition.z+1);
						overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.0f);
						overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder +(int)cellWorldPosition.z;
						overlayTile.GridLocation = tileLocation;

						overlayTile.WorldPosition = new Position
						{
							Left = new Vector3(overlayTile.transform.position.x - 0.5f, overlayTile.transform.position.y, 0),
							Right = new Vector3(overlayTile.transform.position.x + 0.5f, overlayTile.transform.position.y, 0),
							Up = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y + 0.25f, 0),
							Down = new Vector3(overlayTile.transform.position.x, overlayTile.transform.position.y - 0.25f, 0),
						};

						if (UnwalkableTiles.Find(c => c == tile))
							overlayTile.Walkable = false;
						else
							overlayTile.Walkable = true;

						if (ClimbableTiles.Find(c => c == tile))
							overlayTile.Climbable = true;
						else
							overlayTile.Climbable = false;


						MapManager.Instance.Map.Add(tileKey, overlayTile);
					}
				}
			}
		}

		foreach(var map in MapManager.Instance.Map.Values)
			map.GetNeightbourTiles(new List<BaseTile>());
	}
}