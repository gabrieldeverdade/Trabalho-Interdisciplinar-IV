using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour
{
	public float Speed = 10;

	bool IsMoving = false;
	[SerializeField] Character Character;
	[SerializeField] int StartPositionX;
	[SerializeField] int StartPositionY;

	List<BaseTile> Path = new();
	PathFinder PathFinder;

	private void Start()
	{
		PathFinder = new PathFinder(true);
	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (!IsMoving)
		{
			if(Character.ActiveTile == null)
				Character.ActiveTile = MapManager.Instance.Map[new Vector2Int(StartPositionX, StartPositionY)];

			if (Input.GetKey("d"))
			{
				if (Character.ActiveTile.HasSouthNeighbour(MapManager.Instance.Map, out var found))
				{
					Path = PathFinder.Find(Character.ActiveTile, found);
					IsMoving = true;
				}
			}

			if (Input.GetKey("w"))
			{
				if (Character.ActiveTile.HasEastNeighbour(MapManager.Instance.Map, out var found))
				{
					Path = PathFinder.Find(Character.ActiveTile, found);
					IsMoving = true;
				}
			}

			if (Input.GetKey("s"))
			{
				if (Character.ActiveTile.HasWestNeighbour(MapManager.Instance.Map, out var found))
				{
					Path = PathFinder.Find(Character.ActiveTile, found);
					IsMoving = true;
				}
			}

			if (Input.GetKey("a"))
			{
				if (Character.ActiveTile.HasNorthNeighbour(MapManager.Instance.Map, out var found))
				{
					Path = PathFinder.Find(Character.ActiveTile, found);
					IsMoving = true;
				}
			}
		}


		if (Input.GetKey("g"))
		{
			foreach (var tile in MapManager.Instance.Map)
				tile.Value.ShowTile();
		}

		if (Input.GetKey("h"))
		{
			foreach (var tile in MapManager.Instance.Map)
				tile.Value.HideTile();
		}


		if (Path.Count > 0 && IsMoving)
			MoveAlongPath();

		if (Path.Count == 0)
			IsMoving = false;
	}

	void MoveAlongPath()
	{
		var step = 2 * Time.deltaTime;
		var firstPosition = Path[0].transform.position;
		var z = firstPosition.z;

		Character.transform.position = Vector2.MoveTowards(Character.transform.position, firstPosition, step);
		Character.transform.position = new Vector3(Character.transform.position.x, Character.transform.position.y, z);

		if (Vector2.Distance(Character.transform.position, firstPosition) < 0.0001f)
		{
			PositionCharacterOnTile(Path[0]);
			Path.RemoveAt(0);
		}
	}

	private void PositionCharacterOnTile(BaseTile overlayTyle)
	{
		Character.transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y+0.00002f, overlayTyle.transform.position.z);
		Character.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y+0.23f+0.00002f, overlayTyle.transform.position.z);
		Character.GetComponentInChildren<SpriteRenderer>().sortingOrder = overlayTyle.GetComponent<SpriteRenderer>().sortingOrder;

		Character.ActiveTile = overlayTyle;
	}
}
