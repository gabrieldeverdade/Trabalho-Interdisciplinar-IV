using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator 
{
	public static void CreateWalls(HashSet<Vector2> floorPositions, TilemapVisualizer tilemapVisualizer)
	{
		var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.CardinalDirectionsList);
		foreach (var wall in basicWallPositions)
		{
			tilemapVisualizer.PaintSigleBasicWall(wall);
		}
	}

	private static HashSet<Vector2> FindWallsInDirections(HashSet<Vector2> floorPositions, List<Vector3> directionList)
	{
		HashSet<Vector2> wallPositions = new();
		foreach (var position in floorPositions)
		{
			foreach(var direction in directionList)
			{
				var neighbourPosition = position + (Vector2)direction;
				if(!floorPositions.Contains(neighbourPosition))
					wallPositions.Add(neighbourPosition);
			}
		}
		return wallPositions;
	}
}
