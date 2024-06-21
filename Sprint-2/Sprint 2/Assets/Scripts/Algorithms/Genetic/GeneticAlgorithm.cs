using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GeneticAlgorithm : SingletonMonoBehaviour<GeneticAlgorithm>
{
	[SerializeField] int Epoch = 0;
	[SerializeField] int PopulationSize = 100;
	[SerializeField] List<float[]> Population = new List<float[]>();
	[SerializeField] RandomGenerator RandomGenerator = new SystemRandomGenerator();

	public List<float[]> GenerateEpoch(IEnumerable<Scenario> scenario, int amountThatCanGenerate)
	{
		if (Epoch == 0) return GenerateFirstEpoch(amountThatCanGenerate);

		var newPopulation = new List<float[]>();
		var elite = scenario.OrderByDescending(x => Merit(x)).SelectMany(sc => sc.EnemiesList).Select(c => c.Members).Take(20);

		newPopulation.AddRange(elite);

		for(var i = 0; i < 60;i ++)
			newPopulation.Add(Sex(0.2, elite.ElementAt(RandomGenerator.Generate(19)), elite.ElementAt(RandomGenerator.Generate(19))));

		for (var i = 0; i < 20; i++)
			newPopulation.Add(Abiogenese());

		Epoch++;

		Population = newPopulation;
		return Population.Take(Mathf.Min(amountThatCanGenerate, PopulationSize)).ToList();
	}

	List<float[]> GenerateFirstEpoch(int amountThatCanGenerate)
	{
		Epoch++;
		Population = new List<float[]>();

		for (var i = 0; i < PopulationSize; i++)
			Population.Add(Abiogenese());

		return Population.Take(amountThatCanGenerate).ToList();
	}

	float[] Abiogenese() 
		=> new float[2] { (float)RandomGenerator.GeneratePercentage(), (float)RandomGenerator.GeneratePercentage() };

	float[] Sex(double shouldMutate, float[] A, float[] B) 
	{
		var newMember = Abiogenese();
		for (var i = 0; i < 2; i++)
			if (shouldMutate < RandomGenerator.GenerateDouble())
				newMember[i] = RandomGenerator.GenerateDouble() > 0.5 ? A[i] : B[i];

		return newMember;
	}

	int Merit(Scenario scenario) 
		=> scenario.AttackersPoints.Sum() + scenario.ResourcersPoints.Sum();

	public override string ToString()
		=> $@"Generation: {Epoch}";
}