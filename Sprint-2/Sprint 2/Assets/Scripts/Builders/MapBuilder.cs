using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapBuilder : SingletonMonoBehaviour<MapBuilder>
{
	public int MapJumpHeight = 1;

	public GameObject overlayContainer;

	public BaseTile overlayPrefab;

	public List<MappableTile> Tiles;
	public List<Resource> Resources;
	public MappableTile DefaultTile;
	public Tilemap Tilemap;

	private void Start()
	{
		var grid = GetComponentInChildren<Grid>();
		Tilemap = grid.GetComponentInChildren<Tilemap>();
		DefaultTile = Tiles.FirstOrDefault(t => !t.OnTopOfGround && t.Walkable);
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
						var mappableTile = Tiles.FirstOrDefault(t => t.Tile == tile);
						if (mappableTile == null) continue;

						var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
						var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

						overlayTile.Walkable = mappableTile.Walkable;
						overlayTile.Resourceable = mappableTile.Resourceable;

						overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y+ (mappableTile.OnTopOfGround ? 0.250001f : 0), cellWorldPosition.z + 1 + (mappableTile.OnTopOfGround ? 1 : 0));
						overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
						overlayTile.GetComponent<SpriteRenderer>().sprite = mappableTile.ValidTiles[0].sprite;
						overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder +(int)cellWorldPosition.z+(mappableTile.OnTopOfGround ? 1 : 0);
						overlayTile.GridLocation = tileLocation;

						MapManager.Instance.Map.Add(tileKey, overlayTile);
					}
				}
			}
		}

		foreach(var map in MapManager.Instance.Map.Values)
			map.GetNeightbourTiles(new List<BaseTile>());
	}
}