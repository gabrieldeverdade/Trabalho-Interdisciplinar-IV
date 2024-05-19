using System.Collections.Generic;
using UnityEngine;

public class MapManager : SingletonMonoBehaviour<MapManager>
{
	public int MapJumpHeight = 1;

	public Dictionary<Vector2Int, BaseTile> Map;
	public Dictionary<Character, List<BaseTile>> Paths = new();

	public void UpdatePath(Character character, List<BaseTile> path)
	{
		if (!Paths.ContainsKey(character))
				Paths.Add(character, new List<BaseTile>());

		Paths[character] = path;
	}


}