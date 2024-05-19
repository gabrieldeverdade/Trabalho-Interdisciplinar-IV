using UnityEngine;

public class Character: MonoBehaviour
{
	public int Health = 50;
	public BaseTile ActiveTile;
	public BaseController Controller;

	public void TakeHit()
	{
		Health -= 10;
		if (Health <= 0)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var tag = collision.gameObject.tag;

		if (tag == "Enemy")
		{
			TakeHit();
			if(Health <= 0)
				Destroy(gameObject);
		}
	}
}