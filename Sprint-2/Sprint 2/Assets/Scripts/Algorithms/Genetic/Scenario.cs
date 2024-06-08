public class Scenario
{
	public int Enemies { get; private set; }
	public int Colectors { get; private set; }
	public int Atackers { get; private set; }
	public int Resources { get; private set; }

	public Scenario(int colectors, int atackers, int resources)
	{
		Enemies = colectors + atackers;
		Colectors = colectors;
		Atackers = atackers;
		Resources = resources;
	}
}
