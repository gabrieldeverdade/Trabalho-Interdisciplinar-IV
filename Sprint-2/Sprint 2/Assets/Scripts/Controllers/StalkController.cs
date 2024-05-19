using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StalkController : BaseController
{
	public float Speed = 3;
	public int Range = 6;

	CharacterMover CharacterMover;

	Character Character;
	[SerializeField] Character Destination;
	[SerializeField] int StartPositionX;
	[SerializeField] int StartPositionY;


	BaseTile Latest;
	PathFinder PathFinder;

	List<BaseTile> Path => Character != null && MapManager.Instance.Paths.ContainsKey(Character) ? MapManager.Instance.Paths[Character] : new List<BaseTile>();

	void Start()
	{
		PathFinder = new PathFinder(false);
		Character = GetComponent<Character>();
		this.AddComponent<CharacterMover>();
		this.AddComponent<CharacterRenderer>();

		CharacterMover = GetComponent<CharacterMover>();
	}

	public void LateUpdate()
	{
		if(Character.ActiveTile == null)
			Character.ActiveTile = MapManager.Instance.Map[new Vector2Int(StartPositionX, StartPositionY)];

		ChangePaths();

		if (Character != null)
		{
			if (Path.Count > 0)
			{
				if(CharacterMover.Move(Speed, Path[0]))
					Path.RemoveAt(0);
			}
		}
	}

	void ChangePaths()
	{
		if (Latest != Destination.ActiveTile)
		{
			var newPath = PathFinder.Find(Character.ActiveTile, Destination.ActiveTile, range: Range);
			MapManager.Instance.UpdatePath(Character, newPath);
			Latest = Destination.ActiveTile;
		}
	}

	void ChangePathsOptimized()
	{
		var range = Range;
		if (Latest != Destination.ActiveTile)
		{
			if (Latest == null || Path.Count <= 3)
			{
				var newPath = PathFinder.Find(Character.ActiveTile, Destination.ActiveTile, range: Range);
				MapManager.Instance.UpdatePath(Character, newPath);
			}
			else
			{
				var newPath = Path ?? new List<BaseTile>();

				if (newPath.Count > range) newPath.RemoveRange(newPath.Count - range, range);

				var latestTile = newPath[newPath.Count - 1];
				var newTile = PathFinder.Find(latestTile, Destination.ActiveTile, range + 1);

				newPath.AddRange(newTile);

				MapManager.Instance.UpdatePath(Character, newPath);
			}
			Latest = Destination.ActiveTile;
		}
	}

	public void SetDestination(Character character)
		=> Destination = character;
}
