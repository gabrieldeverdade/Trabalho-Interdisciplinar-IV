using UnityEngine;

public class Stats: MonoBehaviour
{
	public int Health;
	public int Stamina;

	public bool IsDead => Health <= 0;

	public bool TakeHit(int hit)
	{
		Health -= hit;
		return IsDead;
	}
}