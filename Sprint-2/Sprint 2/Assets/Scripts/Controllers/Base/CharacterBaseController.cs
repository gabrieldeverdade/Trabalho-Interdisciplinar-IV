using UnityEngine;

public class CharacterBaseController<T> : BaseController where T: Character
{
	protected T Character;
	protected CharacterMover CharacterMover;
	protected PlayerAnimation PlayerAnimation;

	void Start()
	{
		CharacterMover = new CharacterMover();
		Character = GetComponent<T>();
		PlayerAnimation = Character.GetComponent<PlayerAnimation>();
	}

	void Update()
	{
		InitializeActiveTile();
	}

	protected bool Move(Vector2Int direction, T character, float speed)
		=> CharacterMover.Move(direction, character, speed);

	protected bool Move(Vector2 direction, T character, float speed)
		=> Move(Vector2Int.RoundToInt(direction), character, speed);

	protected void InitializeActiveTile()
	{
		if (IsCharacterNotNull() && !IsCharacterWithActiveTile())
		{
			Character.ActiveTile = MapManager.Instance.GetCellFromWorldPosition(Character);
			Character.Position = Character.transform.position;
			PlayerAnimation.Play("IdleBottom");
		}
	}

	protected bool IsCharacterValid() => IsCharacterNotNull() && IsCharacterWithActiveTile();
	protected bool IsCharacterNotNull() => Character != null;
	protected bool IsCharacterWithActiveTile() => IsCharacterNotNull() && Character.ActiveTile != null;
}