public class KeyboardController : CharacterBaseController<Character>
{
	public float Speed = 0.1f;

	void FixedUpdate() { Walk(); }

	void Walk()
	{
		if (!IsCharacterValid())
			return;

		var direction = GetInputVector();

		if (Move(direction, Character, Speed))
			return;

		PlayerAnimation.Animate(direction, Speed);
	}
}