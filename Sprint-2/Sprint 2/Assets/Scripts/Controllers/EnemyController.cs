
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : BaseController
{
	bool GoingToResource = false;
	bool GoingBackFromResource = false;
	bool SearchingResource = true;
	bool IsResourceGetter = true;

	public float Speed = 3;
	public int Range = 20;
	bool IsInRange = false;
	bool CanSpawn = false;

	CharacterMover CharacterMover;

	Character Character;
	[SerializeField] float DecisionAmount = 3f;
	[SerializeField] float WhenChangeDirection = 3f;
	[SerializeField] Character Destination;
	[SerializeField] int StartPositionX;
	[SerializeField] int StartPositionY;
	[SerializeField] List<BaseTile> PathList;

	BaseTile Latest;
	PathFinderOptimized PathFinder;
	SpawnArea SpawnArea;

	List<BaseTile> Path => Character != null && MapManager.Instance.Paths.ContainsKey(Character) ? MapManager.Instance.Paths[Character] : new List<BaseTile>();


	void Start()
	{
		PathFinder = new PathFinderOptimized();
		SpawnArea = GetComponentInParent<SpawnArea>();
		Character = GetComponent<Character>();

		this.AddComponent<CharacterMover>();
		this.AddComponent<CharacterRenderer>();

		IsResourceGetter = false;/*Random.Range(0, 10) > 3;*/

		CharacterMover = GetComponent<CharacterMover>();
	}

	void Update()
	{
		if (CanSpawn && Character.ActiveTile == null)
		{
			CanSpawn = false;
			Character.ActiveTile = MapManager.Instance.Map[new Vector2Int(StartPositionX, StartPositionY)];
		}

		if (Character != null)
		{
			ChangePaths();

			if (Path.Count > 0)
			{
				if (CharacterMover.Move(Speed, Path[0]))
				{
					Character.ActiveTile = Path[0];
					Path.RemoveAt(0);
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		for(var i = 0; i < Path.Count - 2; i++)
		{
			var current = Path[i];
			var next = Path[i+1];
			Gizmos.DrawLine(current.transform.position, next.transform.position);
		}
	}

	void ChangePaths()
	{
		if (IsResourceGetter)
		{
			DecisionAmount += Time.deltaTime;

			if ((GoingToResource || GoingBackFromResource) && Path.Count > 0) return;

			if (GoingToResource)
			{
				GoingBackFromResource = true;
				GoingToResource = false;
				if (SpawnArea.GetResource(out var resource))
				{
					resource.Reverse();
					MapManager.Instance.UpdatePath(Character, resource);
				}
				Debug.Log("GOING BACK THROUGH RESOURCE PATH");
			} else if (GoingBackFromResource)
			{
				GoingBackFromResource = false;
				GoingToResource = true;
				if (SpawnArea.GetResource(out var resource))
					MapManager.Instance.UpdatePath(Character, resource);
				Debug.Log("GOING THROUGH RESOURCE PATH");
			}

			if (!MakeResourceFindMovement())
				MakeRandomMovement();

			return;
		} 
		else
		{
			if (Destination != null && Character.ActiveTile != null && Latest != Destination.ActiveTile)
				MakeStalkMovement();
			else if (!IsInRange && DecisionAmount > WhenChangeDirection)
				MakeRandomMovement();
		}
		
		if (PathList.Count == 0 && IsInRange) IsInRange = false;
	}

	public void MakeRandomMovement()
	{
		if(DecisionAmount > WhenChangeDirection)
		{
			DecisionAmount = 0;
			var random = new System.Random();
			GetComponent<Rigidbody2D>().velocity = new Vector2(random.Next(-1, 2), random.Next(-1, 2));
		}
	}

	public void MakeStalkMovement()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		IsInRange = true;
		PathList = PathFinder.Find(Character.ActiveTile, Destination.ActiveTile, range: Range);

		Debug.Log(PathList);
		MapManager.Instance.UpdatePath(Character, PathList);
		Latest = Destination.ActiveTile;
	}

	public void MakeResourceConsumeMovement()
	{
		if(SpawnArea.GetResource(out var resource))
		{
			MapManager.Instance.UpdatePath(Character, resource);
		}
	}

	public bool MakeResourceFindMovement()
	{
		var worldPosition = MapBuilder.Instance.Tilemap.WorldToCell(new Vector3(Character.transform.position.x, Character.transform.position.y, 0));
		var mapPosition = new Vector2Int(worldPosition.x, worldPosition.y);

		if (!MapManager.Instance.Map.ContainsKey(mapPosition)) return false;

		var currentTile = MapManager.Instance.Map[mapPosition];
		Character.GetComponent<CharacterRenderer>().RenderOnTile(currentTile);

		var foundPath = SpawnManager.Instance.HasSeenAnyArea(currentTile, 4);
		if (foundPath.Count > 0)
		{
			var completePath = foundPath;
			var closesTile = Character.ActiveTile.Neighbours.Values.FirstOrDefault(c => c == foundPath[0]);
			if (foundPath.Count == 1 && closesTile != null)
			{
				completePath = SpawnArea.SetResource(closesTile);
				GoingBackFromResource = true;
			}
			else
				Debug.Log($"NOT AT RESOURCE {foundPath.Count} / {currentTile}");

			MapManager.Instance.UpdatePath(Character, completePath);
			IsInRange = true;
			return true;
		}

		return false;
	}

	public void SetStartPosition(int x, int y) 
	{
		CanSpawn = true;
		StartPositionX = x;
		StartPositionY = y;
	}

	public void SetDestination(Character character)
		=> Destination = character;
}
