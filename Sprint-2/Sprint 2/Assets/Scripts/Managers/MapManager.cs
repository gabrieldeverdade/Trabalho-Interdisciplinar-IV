using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : SingletonMonoBehaviour<MapManager>
{
	public string GrassTile;
	public bool ignoreBottomTiles;
	public int MapJumpHeight = 1;

	public GameObject overlayContainer;

	public BaseTile overlayPrefab;
	public Dictionary<Vector2Int, BaseTile> Map;
	public Dictionary<Character, List<BaseTile>> Paths = new();

	public Resource RockResource;
	public Resource WaterResource;
	public Resource GrassResource;

	private void Start()
	{
		BuildTilemapContainer(gameObject.GetComponentInChildren<Tilemap>());
	}

	public void UpdatePath(Character character, List<BaseTile> path)
	{
		if (!Paths.ContainsKey(character))
				Paths.Add(character, new List<BaseTile>());

		//ClearPath();
		Paths[character] = path;
		AfterPathUpdate(path);
	}

	public void ClearPath()
	{
		foreach (var tile in Map.Values)
			tile.HideTile();
	}

	void AfterPathUpdate(List<BaseTile> tiles) 
	{
		foreach (var tile in tiles)
			tile.ShowTile();
	}

	// Start is called before the first frame update
	void BuildTilemapContainer(Tilemap tileMap)
	{
		Map = new Dictionary<Vector2Int, BaseTile>();

		BoundsInt bounds = tileMap.cellBounds;

		for (int z = bounds.max.z; z >= bounds.min.z; z--)
		{
			for (int y = bounds.min.y; y < bounds.max.y; y++)
			{
				for (int x = bounds.min.x; x < bounds.max.x; x++)
				{
					if (z == 0 && ignoreBottomTiles)
						return;

					var tileLocation = new Vector3Int(x, y, z);
					var tileKey = new Vector2Int(x, y);
					if (tileMap.HasTile(tileLocation) && !Map.ContainsKey(tileKey))
					{
						var tile = tileMap.GetTile(tileLocation);


						var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);

						if (tile.name == "GrassTile")
							overlayTile.Resource = GrassResource;
						else if (tile.name == "RockTile")
							overlayTile.Resource = RockResource;
						else if (tile.name == "WaterTile")
							overlayTile.Resource = WaterResource;

						var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

						overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
						overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
						overlayTile.GridLocation = tileLocation;
						overlayTile.HideTile();

						Map.Add(tileKey, overlayTile);
					}
				}
			}
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}