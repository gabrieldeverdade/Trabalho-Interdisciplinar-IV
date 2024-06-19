
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : SingletonMonoBehaviour<EnemyManager>
{
	[SerializeField]List<SpawnArea> Enemies; 

	GeneticAlgorithm GeneticAlgorithm;

	public int CurrentGeneration;
	public int TimeUntilNextGeneration = 90;
	public float CurrentGenerationTime = 0;

	void Start()
	{
		GeneticAlgorithm = new GeneticAlgorithm();
	}

	void Update()
	{
		CurrentGenerationTime += Time.deltaTime;

		if(CurrentGenerationTime > TimeUntilNextGeneration)
		{
			CurrentGenerationTime = 0;
		}
	}

	void FixedUpdate()
	{
		
	}
}