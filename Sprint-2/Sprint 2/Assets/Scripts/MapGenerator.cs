using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : AbstractMapGenerator
{

	[SerializeField] private SimpleRandomWakData RandomWalkParameters;

	protected override void RunProceduralGeneration()
	{
		HashSet<Vector2> floorPositions = RunRandomWalk();
		TilemapVisualizer.PaintFloorTiles(floorPositions);
		WallGenerator.CreateWalls(floorPositions, TilemapVisualizer);
	}

	protected HashSet<Vector2> RunRandomWalk()
	{
		var currentPosition = StartPosition;
		HashSet<Vector2> floorPositions = new();
		for (int i = 0; i < RandomWalkParameters.Iterations; i++)
		{
			var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, RandomWalkParameters.WalkLength);
			floorPositions.UnionWith(path);

			if (RandomWalkParameters.StartRandomlyEachIteration)
				currentPosition = floorPositions.ElementAt(UnityEngine.Random.Range(0, floorPositions.Count()));
		}
		return floorPositions;
	}
}
