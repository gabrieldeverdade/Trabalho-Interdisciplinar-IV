using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StalkController : MonoBehaviour
{
	public float Speed = 5;
	public int Range = 5;

	bool IsMoving = false;
	[SerializeField] Character Character;
	[SerializeField] Character Destination;
	[SerializeField] int StartPositionX;
	[SerializeField] int StartPositionY;

	BaseTile Latest;
	PathFinder PathFinder;

	List<BaseTile> Path => Character != null && MapManager.Instance.Paths.ContainsKey(Character) ? MapManager.Instance.Paths[Character] : new List<BaseTile>();

	private void Start()
	{
		PathFinder = new PathFinder(false);
		//StartCoroutine(ChangePaths());
	}

	void LateUpdate()
	{
		if(Character.ActiveTile == null)
			Character.ActiveTile = MapManager.Instance.Map[new Vector2Int(StartPositionX, StartPositionY)];

		ChangePaths();
		if (Character != null)
		{
			if (Path.Count > 0)
				MoveAlongPath();

			if (Path.Count == 0)
				IsMoving = false;
		}
	}

	void ChangePaths()
	{
		
		if (Latest != Destination.ActiveTile)
		{
			if(Latest == null) 
			{
				var newPath = PathFinder.Find(Character.ActiveTile, Destination.ActiveTile, range: 20);
				MapManager.Instance.UpdatePath(Character, newPath);
			}
			else
			{
				var newTile = PathFinder.Find(Latest, Destination.ActiveTile, range: 1);
				var newPath = Path ?? new List<BaseTile>();

				if (newTile.Count == 0)
					newPath.RemoveAt(Path.Count - 1);
				else
					newPath.AddRange(newTile);

				MapManager.Instance.UpdatePath(Character, newPath);
			}
			Latest = Destination.ActiveTile;
		}
	}

	void MoveAlongPath()
	{
		var step = Speed * Time.deltaTime;
		var firstPosition = Path[0].transform.position;
		var z = firstPosition.z;

		Character.transform.position = Vector2.MoveTowards(Character.transform.position, firstPosition, step);
		Character.transform.position = new Vector3(Character.transform.position.x, Character.transform.position.y, z);

		if (Vector2.Distance(Character.transform.position, firstPosition) < 0.0001f)
		{
			PositionCharacterOnTile(Path[0]);
			Path[0].HideTile();
			Path.RemoveAt(0);
		}
	}

	void PositionCharacterOnTile(BaseTile overlayTyle)
	{
		Character.transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y+0.00002f, overlayTyle.transform.position.z);
		Character.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y+0.23f+0.00002f, overlayTyle.transform.position.z);
		Character.GetComponentInChildren<SpriteRenderer>().sortingOrder = overlayTyle.GetComponent<SpriteRenderer>().sortingOrder;

		Character.ActiveTile = overlayTyle;
	}

	public void SetStartPosition(int x, int y)
		=> PositionCharacterOnTile(MapManager.Instance.Map[new Vector2Int(x, y)]);

	public void SetDestination(Character character)
		=> Destination = character;
}
