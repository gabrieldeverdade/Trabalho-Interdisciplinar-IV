
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea: MonoBehaviour
{
	[SerializeField] int SpawnX;
	[SerializeField] int SpawnY;
	[SerializeField] int TimeToSpawn;

	BaseTile Tile;

	[SerializeField] List<Character> Enemies;
	[SerializeField] Character EnemyPrefab;
	[SerializeField] Character RandomWalkEnemyPrefab;
	[SerializeField] Character Destination;

	void Start()
	{
	}

	void Update()
	{
		if(Tile == null)
		{
			Tile = MapManager.Instance.Map[new Vector2Int(SpawnX, SpawnY)];
			for(int i = 0; i < 2; i++)
				Spawn();

			for (int i = 0; i < 2; i++)
				SpawnRandomWalk();
		}
	}

	void Spawn()
	{
		var newChar = Instantiate(EnemyPrefab);
		newChar.GetComponent<StalkController>().SetDestination(Destination);
		newChar.GetComponent<StalkController>().SetStartPosition(SpawnX, SpawnY);
		Enemies.Add(newChar);
	}

	void SpawnRandomWalk()
	{
		var newChar = Instantiate(RandomWalkEnemyPrefab);
		newChar.GetComponent<RandomMoveController>().SetStartPosition(SpawnX, SpawnY);
		Enemies.Add(newChar);
	}
}