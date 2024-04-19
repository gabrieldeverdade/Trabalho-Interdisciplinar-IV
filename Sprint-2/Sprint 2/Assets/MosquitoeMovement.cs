using UnityEngine;

public class MosquitoeMovement : MonoBehaviour
{
	private Rigidbody2D rb;
	private float moveH, moveV;
	[SerializeField] private float moveSpeed = 1.0f;

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update()
	{
		moveH = Random.Range(-1, 1) * moveSpeed;
		moveV = Random.Range(-1, 1) * moveSpeed;
		rb.velocity = new Vector2(moveH, moveV);
	}
}
