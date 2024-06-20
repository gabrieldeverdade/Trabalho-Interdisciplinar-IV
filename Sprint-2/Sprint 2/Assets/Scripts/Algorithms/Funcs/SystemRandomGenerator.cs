
using System;

public class SystemRandomGenerator : RandomGenerator
{
	Random Random;

	public SystemRandomGenerator()
	{
		Random = new Random(42);
	}

	public override int Generate(int start, int end) => Random.Next(start, end);
	public override int Generate(int end) => Random.Next(end);
	public override double GenerateDouble() => Random.NextDouble();

	public override double GeneratePercentage() => Random.NextDouble();
}
