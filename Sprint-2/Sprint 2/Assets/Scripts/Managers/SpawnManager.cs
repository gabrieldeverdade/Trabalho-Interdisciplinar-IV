using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager: SingletonMonoBehaviour<SpawnManager>
{
	public SpawnArea[] SpawnAreas;
	public Text Text;

	public float SpawnTimer = 10;
	public float TimeUntilSpawn = 0;


	private void Update()
	{
		TimeUntilSpawn += Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.Q) || TimeUntilSpawn > SpawnTimer)
		{
			TimeUntilSpawn = 0;
			var scenarios = SpawnAreas.Select(c => new Scenario(c.Enemies));
			var items = GeneticAlgorithm.Instance.GenerateEpoch(scenarios, 5);

			if(Text != null)
				Text.text = GeneticAlgorithm.Instance.ToString();

			foreach (var spawn in SpawnAreas)
				spawn.Spawn(items);
		}
	}
}