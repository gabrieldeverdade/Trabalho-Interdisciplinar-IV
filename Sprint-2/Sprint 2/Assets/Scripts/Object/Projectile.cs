using Unity.VisualScripting;
using UnityEngine;

public class Projectile: MonoBehaviour
{
	public float Speed = 2;

	public float TimeToLive = 1f;
	float TimeSinceSpawned = 0f;

	private void Update()
	{
		transform.position += Speed * transform.right * Time.deltaTime;

		TimeSinceSpawned += Time.deltaTime;
		if(TimeSinceSpawned > TimeToLive)
			Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var tag = collision.gameObject.tag;

		if(tag == "Enemy")
		{
			var character = collision.gameObject.GetComponent<Character>();
			character.TakeHit();
			Destroy(gameObject);
		}
	}

}