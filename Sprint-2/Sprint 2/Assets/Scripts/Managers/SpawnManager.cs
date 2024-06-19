using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager: SingletonMonoBehaviour<SpawnManager>
{
	public SpawnArea[] SpawnAreas;
	public Text Text;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			var scenarios = SpawnAreas.Select(c => new Scenario(c.Enemies));
			var items = GeneticAlgorithm.Instance.GenerateEpoch(scenarios, 5);

			if(Text != null)
				Text.text = GeneticAlgorithm.Instance.ToString();

			foreach (var spawn in SpawnAreas)
				spawn.Spawn(items);
		}
	}
}