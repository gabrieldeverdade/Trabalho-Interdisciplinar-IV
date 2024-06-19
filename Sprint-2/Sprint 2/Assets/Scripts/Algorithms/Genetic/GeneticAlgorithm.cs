using System.Collections.Generic;
using System.Linq;
using UnityEngine;


enum EnemyVariables
{
	PercentageToBeAttacker = 0,
	PercentageToTransformToBeAttacker = 0,
}

public class GeneticAlgorithm : SingletonMonoBehaviour<GeneticAlgorithm>
{
	[SerializeField] int Epoch = 0;
	[SerializeField] int PopulationSize = 100;
	[SerializeField] List<float[]> Population = new List<float[]>();
	[SerializeField] RandomGenerator RandomGenerator = new SystemRandomGenerator();

	public List<float[]> GenerateEpoch(IEnumerable<Scenario> scenario, int amountThatCanGenerate)
	{
		if (Epoch == 0) 
		{
			Epoch++;
			Population = new List<float[]>();

			for (var i = 0; i < PopulationSize; i++)
				Population.Add(Abiogenese());

			return Population.Take(amountThatCanGenerate).ToList();
		}

		var member = new float[2] { 0.5f, 0.6f };
		var newPopulation = new List<float[]>();
		var elite = Population.OrderByDescending(x => Merit(scenario.First())).Take(20);

		newPopulation.AddRange(elite);

		for(var i = 0; i < 60;i ++)
			newPopulation.Add(Sex(0.2, elite.ElementAt(RandomGenerator.Generate(19)), elite.ElementAt(RandomGenerator.Generate(19))));

		for (var i = 0; i < 20; i++)
			newPopulation.Add(Abiogenese());

		Epoch++;

		Population = newPopulation;
		return Population.Take(Mathf.Min(amountThatCanGenerate, PopulationSize)).ToList();
	}

	float[] Abiogenese() 
		=> new float[2] { RandomGenerator.Generate(50, 100), RandomGenerator.Generate(0, 80) };

	float[] Sex(double shouldMutate, float[] A, float[] B) 
	{
		var newMember = Abiogenese();
		for (var i = 0; i < 2; i++)
			if (shouldMutate < RandomGenerator.GenerateDouble())
				newMember[i] = RandomGenerator.GenerateDouble() > 0.5 ? A[i] : B[i];

		return newMember;
	}

	int Merit(Scenario scenario) 
	{
		//var beCollector = new DecisionTreeNode<double>(s => s.Resourcers > member[0])
		//			.When(s => s.Resourcers > member[1], n => n.DecideValue(member[2] / 100, member[3] / 100))
		//			.WhenNot(s => s.Attackers > member[4], n => n.DecideValue(member[5] / 100, member[6] / 100));

		for (var i = 0; i < 100; i++)
		{
			//var enemyClass = beCollector.Decide(scenario) > RandomGenerator.GenerateDouble() ? "Colector" : "Atacker";
			//Debug.Log($"Resources: {scenario.Resources} Collectors: {scenario.Colectors} Chance: {beCollector.Decide(scenario) * 100}% class: {enemyClass}");
		}
		return 0;
	}

	public override string ToString()
		=> $@"Generation: {Epoch}";
}