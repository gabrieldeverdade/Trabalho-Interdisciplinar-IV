
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
	[SerializeField] Character Destination;

	void Start()
	{
	}

	void Update()
	{
		if(Tile == null)
		{
			Tile = MapManager.Instance.Map[new Vector2Int(SpawnX, SpawnY)];
			Spawn();
		}
	}

	void Spawn()
	{
		var newChar = Instantiate(EnemyPrefab);
		newChar.GetComponent<StalkController>().SetDestination(Destination);
		newChar.GetComponent<StalkController>().SetStartPosition(SpawnX, SpawnY);
		Enemies.Add(newChar);
	}
}