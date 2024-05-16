using System.Collections.Generic;
using UnityEngine;
public class RandomMoveController : MonoBehaviour
{
	public float Speed = 5;
	public int Range = 5;

	bool IsMoving = false;
	[SerializeField] Character Character;

	[SerializeField] int StartPositionX;
	[SerializeField] int StartPositionY;

	BaseTile Latest;
	PathFinder PathFinder;
	List<BaseTile> Path => Character != null && MapManager.Instance.Paths.ContainsKey(Character) ? MapManager.Instance.Paths[Character] : new List<BaseTile>();

	void Start()
	{
		PathFinder = new PathFinder(false);
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
		if (Path.Count == 0)
		{
			var randomTile = Random.Range(0, 4);
			var tile = Character.ActiveTile.GetNeightbourTiles(new List<BaseTile>())[randomTile];
			MapManager.Instance.UpdatePath(Character, new List<BaseTile> { tile });
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
}
