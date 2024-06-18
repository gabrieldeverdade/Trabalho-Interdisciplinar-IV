using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class MapManager : SingletonMonoBehaviour<MapManager>
{
	[SerializeField] Text Text;

	public int Points { get; private set; }
	public bool Debug { get; set; }

	public Dictionary<Vector2Int, BaseTile> Map;
	public Dictionary<Character, List<BaseTile>> Paths = new();

	[SerializeField] public Tile WIPTile;

	private void Start()
	{
		if(Text != null) Text.text = Points.ToString();
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

		if(Text != null)
		Text.text = Points.ToString();
	}

	public BaseTile GetCellFromWorldPosition(Character character)
	{
		var tile = GetCellFromWorldPosition(character.transform.position);
		return tile != null ? tile : character.ActiveTile;
	}

	public BaseTile GetCellFromWorldPosition(Vector3 position)
	{
		var worldPosition = MapBuilder.Instance.Tilemap.WorldToCell(position);
		var worldPosition2D = new Vector2Int(worldPosition.x, worldPosition.y);

		if (MapManager.Instance.Map.TryGetValue(worldPosition2D, out var nextTile))
			return nextTile;

		return null;
	}

	public int GetRow(Vector3 position)
		=> MapBuilder.Instance.Tilemap.WorldToCell(position).x;

}