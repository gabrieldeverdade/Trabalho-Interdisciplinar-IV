using System.Collections.Generic;
using UnityEngine;

public static class ProceduralGenerationAlgorithms
{
	public static HashSet<Vector2> SimpleRandomWalk(Vector2 startPosition, int walkLength)
	{
		HashSet<Vector2> path = new HashSet<Vector2> { startPosition };
		var previousPosition = startPosition;

		for(int i = 0; i < walkLength; i++)
		{
			var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
			path.Add(newPosition);
			previousPosition = newPosition;
		}
		return path;
	}
}

public static class Direction2D
{
	public static List<Vector3> CardinalDirectionsList = new List<Vector3>
	{
		new Vector2(0,1) * new Vector3(0.5f, 0.25f, 0.5f),
		new Vector2(1,0)* new Vector3(0.5f, 0.25f, 0.5f),
		new Vector2(0,-1)* new Vector3(0.5f, 0.25f, 0.5f),
		new Vector2(-1,0)* new Vector3(0.5f, 0.25f, 0.5f)
	};

	public static Vector2 GetRandomCardinalDirection()
		=> CardinalDirectionsList[Random.Range(0, CardinalDirectionsList.Count)];
}