using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileLauncher: MonoBehaviour
{
	public float Speed = 3f;
	public float TimeUntilShoot = 0f;
	public bool CanShoot = true;
	public int Bullets = 10;

	public GameObject ProjectilePrefab;
	public Transform SpawnLocation;
	public Quaternion SpawnRotation;
	KeyboardController KeyboardController;

	public void Start()
	{
		KeyboardController = GetComponentInParent<KeyboardController>();
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space) && CanShoot)
		{
			CanShoot = false;
			Bullets--;

			var pos = new Vector3(SpawnLocation.position.x, SpawnLocation.position.y + 0.25f, SpawnLocation.position.z + 2);
			Instantiate(ProjectilePrefab, pos, GetRotation(KeyboardController.CurrentDirection));
		}

		if(Bullets > 0 && !CanShoot)
		{
			TimeUntilShoot += Time.deltaTime;
			if(TimeUntilShoot > Speed) 
			{
				CanShoot = true;
				TimeUntilShoot = 0;
			}
		}
	}

	Quaternion GetRotation(Direction direction) => direction switch
	{
		Direction.N => new Quaternion(-1, -1, 0, 0),
		Direction.NW => new Quaternion(0.25f, 1, 0, 0),
		Direction.NE => new Quaternion(-1, 0.25f, 0, 0),
		Direction.S => new Quaternion(1, -1, 0, 0),
		Direction.SE => new Quaternion(-1, .25f, 0, 0),
		Direction.SW => new Quaternion(-0.25f, 1, 0, 0),
		Direction.E => new Quaternion(0, 0, 0, 0),
		Direction.W => new Quaternion(0, -1, 0, 0),
		_ => new Quaternion(0,0,0,0),
	};

}