using UnityEngine;

public class SpawnArea: MonoBehaviour
{
	[SerializeField] public BaseTile SpawnTile;
	[SerializeField] Transform SpawnLocation;
	[SerializeField] Character EnemyPrefab;
	[SerializeField] Character Destination;

	public float SpawnTimer = 2;
	public float TimeUntilSpawn = 0;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			var enemy = Instantiate(EnemyPrefab, SpawnLocation);
			enemy.GetComponent<CharacterStalkController>().SetDestination(Destination);
			enemy.GetComponent<Enemy>().Spawn = this;
		}
	}

	public void Spawn(int[] enemies)
	{
		for(var i = 0; i < enemies.Length; i++)
		{
			var enemy = Instantiate(EnemyPrefab, SpawnLocation);
			//enemy.GetComponent<CharacterStalkController>().SetDestination(Destination);
			enemy.GetComponent<Enemy>().Spawn = this;
			//enemy.GetComponent<Enemy>().SetJob();
		}
	}
}