
using UnityEngine;

public class CharacterStalkController : BaseController
{
	Enemy Enemy;
	public Character Destination;

	void Start()
	{
		Enemy = GetComponent<Enemy>();
	}

	void FixedUpdate()
	{
		Debug.Log("Searching");
		InitializeActiveTile();
		TryMoveEnemy();
		FindNewPath();
	}

	void InitializeActiveTile() { if (Enemy.ActiveTile == null) Enemy.ActiveTile = MapManager.Instance.GetCellFromWorldPosition(Enemy); }

	void TryMoveEnemy() { if(Path.Count > 0) new TileSpecificMover().Move(Enemy, 1, Path[0]); }

	void FindNewPath() { if (Enemy != null && Enemy.ActiveTile != null) FindPathBetween(Enemy, Destination); }

	public void SetDestination(Character character) => Destination = character;
}
