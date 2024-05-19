using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class RandomMover: MonoBehaviour
{
	public float DecisionClock = 11;
	public float ChangeDecisionAt = 10;
	public float Speed = 10;


	public void Start()
	{
		var random = new System.Random();
		GetComponent<Rigidbody2D>().velocity = new Vector2(random.Next(-1, 2), random.Next(-1, 2)) * Speed;
	}

	public void Update()
	{
		DecisionClock += Time.deltaTime;
		if(DecisionClock > ChangeDecisionAt)
		{
			DecisionClock = 0;
			var random = new System.Random();
			GetComponent<Rigidbody2D>().velocity = new Vector2(random.Next(-1, 2), random.Next(-1, 2)) * Speed;
		}
	}
}