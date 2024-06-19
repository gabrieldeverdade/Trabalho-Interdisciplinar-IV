using System.Collections.Generic;
using System.Linq;

public class Scenario
{
	public int Enemies { get; private set; }
	public int Attackers { get; private set; }
	public IEnumerable<int> AttackersPoints { get; }
	public int Resourcers { get; private set; }
	public IEnumerable<int> ResourcersPoints { get; private set; }

	public Scenario(List<Enemy> enemies)
	{
		Enemies = enemies.Count;
		Resourcers = enemies.Count(c => !c.IsAttacker); ;
		ResourcersPoints = enemies.Where(c => !c.IsAttacker).Select(c => c.Points);
		Attackers = Enemies - Resourcers;
		AttackersPoints = enemies.Where(c => c.IsAttacker).Select(c => c.Points);
	}
}
