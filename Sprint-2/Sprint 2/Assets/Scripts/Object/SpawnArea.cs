using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnArea: MonoBehaviour
{
	[SerializeField] public BaseTile SpawnTile;
	[SerializeField] Transform SpawnLocation;
	[SerializeField] Character EnemyPrefab;
	[SerializeField] Character Destination;

	public List<Enemy> Enemies = new List<Enemy>();

	public float SpawnTimer = 2;
	public float TimeUntilSpawn = 0;

	private void Update()
	{
		InitializeActiveTile();
	}

	public void Spawn(List<float[]> enemies)
	{
		for(var i = 0; i < enemies.Count; i++)
		{
			var enemy = Instantiate(EnemyPrefab, SpawnLocation);

			bool isAttacker = Random.Range(0f, 1f) > 0.5;
			enemy.GetComponent<Enemy>().SetJob(this, isAttacker);

			if (isAttacker)
			{
				enemy.GetComponent<ResourceFinderController>().enabled = false;
				enemy.GetComponent<CharacterStalkController>().enabled = true;
				enemy.GetComponent<CharacterStalkController>().SetDestination(Destination);
			}
			else
			{
				enemy.GetComponent<ResourceFinderController>().enabled = true;
				enemy.GetComponent<CharacterStalkController>().enabled = false;
			}
			Enemies.Add(enemy.GetComponent<Enemy>());
		}
	}

	void InitializeActiveTile() { if (SpawnTile == null) SpawnTile = MapManager.Instance.GetCellFromWorldPosition(SpawnLocation.position); }

}
