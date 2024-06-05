using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : SingletonMonoBehaviour<MapManager>
{
	[SerializeField] Text Text;

	public int Points { get; private set; }

	public Dictionary<Vector2Int, BaseTile> Map;
	public Dictionary<Character, List<BaseTile>> Paths = new();

	private void Start()
	{
		Text.text = Points.ToString();
	}

	public void UpdatePath(Character character, List<BaseTile> path)
	{
		if (!Paths.ContainsKey(character))
				Paths.Add(character, new List<BaseTile>());

		Paths[character] = path;
	}

	public void AddPoints(int point)
	{
		Points += point;
		Text.text = Points.ToString();
	}
}