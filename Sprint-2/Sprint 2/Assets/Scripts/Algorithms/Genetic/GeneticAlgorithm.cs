using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : SingletonMonoBehaviour<GeneticAlgorithm>
{
	public int Epoch = 0;
	public int PopulationSize = 10;
	public List<int[]> Population;
	public RandomGenerator RandomGenerator = new SystemRandomGenerator();

	public void Startup()
	{
		Population = new List<int[]>();

		for (var i = 0; i < PopulationSize; i++)
			Population.Add(Abiogenese());
	}

	public void GenerateEpoch()
	{
		for (var epochs = 0; epochs < 1000; epochs++)
		{
			var scenario = new Scenario(Random.Range(0, 10), Random.Range(0, 10), Random.Range(50, 200));
			var member = new float[2] { 0.5f, 0.6f };
			
			var elite = Population.OrderByDescending(x => Merit(x, scenario)).Take(20);

			Population.Clear();
			Population.AddRange(elite);

			for(var i = 0; i < 60;i ++)
				Population.Add(Sex(0.2, elite.ElementAt(RandomGenerator.Generate(19)), elite.ElementAt(RandomGenerator.Generate(19))));

			for (var i = 0; i < 20; i++)
				Population.Add(Abiogenese());
		}
		//population.Take(1);
		//population.Take(1);
	}

	int[] Abiogenese() 
		=> new int[7] { RandomGenerator.Generate(50, 100), RandomGenerator.Generate(0, 80), 30, 20, 3, 60, 70 };

	int[] Sex(double shouldMutate, int[] A, int[] B) 
	{
		var newMember = Abiogenese();
		for (var i = 0; i < 7; i++)
			if (shouldMutate < RandomGenerator.GenerateDouble())
				newMember[i] = RandomGenerator.GenerateDouble() > 0.5 ? A[i] : B[i];

		return newMember;
	}

	int Merit(int[] member, Scenario scenario) 
	{
		var beCollector = new DecisionTreeNode<double>(s => s.Resources > member[0])
					.When(s => s.Colectors > member[1], n => n.DecideValue(member[2] / 100, member[3] / 100))
					.WhenNot(s => s.Colectors > member[4], n => n.DecideValue(member[5] / 100, member[6] / 100));

		for (var i = 0; i < 100; i++)
		{
			var enemyClass = beCollector.Decide(scenario) > RandomGenerator.GenerateDouble() ? "Colector" : "Atacker";
			//Debug.Log($"Resources: {scenario.Resources} Collectors: {scenario.Colectors} Chance: {beCollector.Decide(scenario) * 100}% class: {enemyClass}");
		}
		return 0;
	}
}


enum EnemyVariables
{
	PercentageToBeAttacker = 0,
	PercentageToTransformToBeAttacker = 0,
}
