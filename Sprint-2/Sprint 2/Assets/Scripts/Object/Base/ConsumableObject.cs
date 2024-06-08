public abstract class ConsumableObject
{
	public int TotalAmount;
	public int CurrentAmount;

	public void Consume(int work)
	{
		if(CanConsume())
			CurrentAmount -= work;
	}

	public void Restart()
	{
		CurrentAmount = TotalAmount;
	}

	public abstract bool CanConsume();
}