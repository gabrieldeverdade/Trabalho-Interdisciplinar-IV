using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private float moveH, moveV;
	[SerializeField] private float moveSpeed = 1.0f;

	// Start is called before the first frame update
	void Start()
	{ 
		rb = GetComponent<Rigidbody2D>();
	}

	public void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("COLLIDED");
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		moveH = Input.GetAxis("Horizontal") * moveSpeed;
		moveV = Input.GetAxis("Vertical") * moveSpeed;
		rb.velocity = new Vector2 (moveH, moveV);

		Vector2 direction = new Vector2(moveH, moveV);
		FindObjectOfType<PlayerAnimation>().SetDirection(direction);
	}
}
