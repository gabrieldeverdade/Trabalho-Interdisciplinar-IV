using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnArea: MonoBehaviour
{
	[SerializeField] public BaseTile SpawnTile;
	[SerializeField] Transform SpawnLocation;
	[SerializeField] Character EnemyPrefab;
	[SerializeField] Character Destination;
	[SerializeField] Dictionary<BaseTile, List<BaseTile>> ResourcesPath = new();

	[SerializeField] List<BaseTile> Resources => ResourcesPath.Keys.ToList();

	public bool IsKeyDown = false;
	public float SpawnTimer = 2;
	public float TimeUntilSpawn = 0;
	public int SpawnX;
	public int SpawnY;

	void Start()
	{
	}
	void Update()
	{
		if (SpawnTile == null)
			SpawnTile = MapManager.Instance.Map[(Vector2Int)MapBuilder.Instance.Tilemap.WorldToCell(SpawnLocation.position)];

		TimeUntilSpawn += Time.deltaTime;

		if(TimeUntilSpawn > SpawnTimer)
		{
			TimeUntilSpawn = 0;
			var enemy = Instantiate(EnemyPrefab, SpawnLocation);
			enemy.GetComponent<EnemyController>().SetDestination(Destination);
			enemy.GetComponent<EnemyController>().SetStartPosition((int)SpawnLocation.position.x, (int)SpawnLocation.position.y);
		}

		if (Input.GetKeyDown(KeyCode.Q) && !IsKeyDown)
		{
			IsKeyDown = true;
			var enemy = Instantiate(EnemyPrefab, SpawnLocation);
			enemy.GetComponent<EnemyController>().SetDestination(Destination);
			
			enemy.GetComponent<EnemyController>().SetStartPosition((int)SpawnTile.transform.position.x, (int)SpawnTile.transform.position.y);
		}

		if (Input.GetKeyUp(KeyCode.Q)) IsKeyDown = false;
	}

	public bool GetResource(out List<BaseTile> resource)
	{
		resource = new List<BaseTile>();
		if (ResourcesPath.Any())
		{
			resource = ResourcesPath.First().Value.CloneViaFakeSerialization();
			return true;
		}
		return false;
	}

	public List<BaseTile> SetResource(BaseTile end)
	{
		var pathFinder = new PathFinderOptimized();
		var path = pathFinder.Find(SpawnTile, end, 20);

		if (ResourcesPath.TryAdd(end, path))
			ResourcesPath[end] = path;
		return path.CloneViaFakeSerialization();
	}

	private void OnDrawGizmos()
	{
		if (ResourcesPath.Any())
		{
			foreach(var resourcePath in ResourcesPath)
			{
				var path = resourcePath.Value;

				for (var i = 0; i < path.Count - 2; i++)
				{
					var current = path[i];
					var next = path[i + 1];
					Gizmos.color = new Color(i/255, 0, 0);
					Gizmos.DrawLine(current.transform.position, next.transform.position);
				}
			}
		}
	}
}