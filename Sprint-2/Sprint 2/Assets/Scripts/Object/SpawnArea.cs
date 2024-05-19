using UnityEngine;

public class SpawnArea: MonoBehaviour
{
	[SerializeField] Transform SpawnLocation;
	[SerializeField] Character EnemyPrefab;
	public bool IsKeyDown = false;
	public float SpawnTimer = 2;
	public float TimeUntilSpawn = 0;

	void Update()
	{
		TimeUntilSpawn += Time.deltaTime;

		if(TimeUntilSpawn > SpawnTimer)
		{
			Instantiate(EnemyPrefab, SpawnLocation);
			TimeUntilSpawn =0;
		}


		if (Input.GetKeyDown(KeyCode.Q) && !IsKeyDown)
		{
			IsKeyDown = true;
			Instantiate(EnemyPrefab, SpawnLocation);
		}

		if (Input.GetKeyUp(KeyCode.Q)) IsKeyDown = false;
	}
}