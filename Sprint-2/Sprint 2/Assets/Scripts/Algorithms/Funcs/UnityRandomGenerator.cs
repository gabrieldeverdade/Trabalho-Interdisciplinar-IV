using UnityEngine;

public class UnityRandomGenerator : RandomGenerator
{
	public override int Generate(int start, int end) => Random.Range(start, end);
	public override double GenerateDouble() => Random.Range(0, 1f);
	public override int Generate(int end) => Random.Range(0, end);
	public override double GeneratePercentage() => Random.Range(0, 1f);
}
