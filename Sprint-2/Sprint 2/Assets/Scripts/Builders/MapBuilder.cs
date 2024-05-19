using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

	private void Start()
	{
		var grid = GetComponentInChildren<Grid>();
		var tilemap = grid.GetComponentInChildren<Tilemap>();
		DefaultTile = Tiles.FirstOrDefault(t => !t.OnTopOfGround && t.Walkable);
		BuildTilemapContainer(tilemap);
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

						overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1 + (mappableTile.OnTopOfGround ? 1 : 0));

						overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
						overlayTile.GetComponent<SpriteRenderer>().sprite = mappableTile.ValidTiles[0].sprite;
						overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
						overlayTile.GridLocation = tileLocation;

						MapManager.Instance.Map.Add(tileKey, overlayTile);

						if (mappableTile.OnTopOfGround)
						{
							overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
							cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

							overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);

							overlayTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
							overlayTile.GetComponent<SpriteRenderer>().sprite = DefaultTile.ValidTiles[0].sprite;
							overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
							overlayTile.GridLocation = tileLocation;
						}
					}
				}
			}
		}
		tileMap.enabled = false;
		tileMap.RefreshAllTiles();
	}
}