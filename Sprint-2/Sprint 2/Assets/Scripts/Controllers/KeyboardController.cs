using UnityEngine;

public class KeyboardController : MonoBehaviour
{
	public int Speed = 2;
	public CharacterMover InsideTileMover;

	PlayerAnimation PlayerAnimation;

	[SerializeField] Character Character;

	private void Start()
	{
		PlayerAnimation = Character.GetComponent<PlayerAnimation>();
		InsideTileMover = new CharacterMover();
	}

	private void Update()
	{
		InitializeCharacterActiveTile();
		Walk(InputController.GetInputVector());
	}

	void InitializeCharacterActiveTile()
	{
		if (Character.ActiveTile == null)
		{
			Character.ActiveTile = MapManager.Instance.GetCellFromWorldPosition(Character);
			Character.Position = Character.transform.position;
			PlayerAnimation.Play("IdleBottom");
		}
	}

	void Walk(Vector2Int direction)
	{
		if (Character == null || Character.ActiveTile == null)
			return;

		if (InsideTileMover.Move(direction, Character))
			return;

		PlayerAnimation.Animate(direction, Speed);
	}
}