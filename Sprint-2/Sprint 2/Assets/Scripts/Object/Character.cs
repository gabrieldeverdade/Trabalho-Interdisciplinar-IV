using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
	public int StartHealth = 50;

	public int Health = 50;
	public bool IsPlayer = false;
	public BaseTile ActiveTile;
	public BaseController Controller;
	public bool IsDamaging = false;

	public void TakeHit()
	{
		if (IsPlayer && !IsDamaging)
		{
			StartCoroutine(FlickOnScreen());
			Health -= 10;
			if (Health <= 0)
			{
				Destroy(gameObject);
				SceneManager.LoadScene(2);
			}
		}
		else if (!IsPlayer)
		{
			Health -= 10;
			if (Health <= 0)
			{
				Destroy(gameObject);
				MapManager.Instance.AddPoints(10);

			}
		}

		if(GetComponentInChildren<HealthManager>() != null)
			GetComponentInChildren<HealthManager>().ChangeBar((float)Health / (float)StartHealth);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		var tag = collision.gameObject.tag;

		if (tag == "Enemy" && gameObject.tag != "Enemy")
		{
			TakeHit();
			if (Health <= 0)
				Destroy(gameObject);
		}
	}

	IEnumerator FlickOnScreen()
	{
		IsDamaging = true;
		var i = 4;
		var delay = new WaitForSeconds(0.25f);
		while (i > 0)
		{
			GetComponentInChildren<SpriteRenderer>().color = new Color(1,1,1, i % 2 != 0 ? 0.3f : 1);
			i--;
			yield return delay;
		}
		GetComponentInChildren<SpriteRenderer>().color = new Color(1, 1, 1, 1);

		IsDamaging = false;
	}
}