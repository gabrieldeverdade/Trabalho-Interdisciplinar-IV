using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
	[SerializeField] private Tilemap FloorTilemap;
	[SerializeField] private Tilemap WallTilemap;
	[SerializeField] private TileBase FloorTile;
	[SerializeField] private TileBase WallTile;

	public void PaintFloorTiles(IEnumerable<Vector2> floorPositions)
	{
		PaintTiles(floorPositions, FloorTilemap, FloorTile);
	}
	
	private void PaintTiles(IEnumerable<Vector2> positions, Tilemap tilemap, TileBase tile)
	{
		Clear();
		foreach (var position in positions)
			PaintSingleTile(position, tilemap, tile);
	}

	private void PaintSingleTile(Vector2 position, Tilemap tilemap, TileBase tile)
	{
		var tilePosition = tilemap.WorldToCell(position);
		tilemap.SetTile(tilePosition, tile);
	}

	public void Clear()
	{
		FloorTilemap.ClearAllTiles();
		WallTilemap.ClearAllTiles();
	}

	internal void PaintSigleBasicWall(Vector2 wall)
		=> PaintSingleTile(wall, WallTilemap, WallTile);
}
