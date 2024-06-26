using UnityEngine;

public class InputBaseController : MonoBehaviour
{
	protected (int horizontal, int vertical) GetInput()
	{
		var horizontal = Input.GetAxisRaw("Horizontal") > 0 ? 1 : Input.GetAxisRaw("Horizontal") < 0 ? -1 : 0;
		var vertical = Input.GetAxisRaw("Vertical") > 0 ? 1 : Input.GetAxisRaw("Vertical") < 0 ? -1 : 0;
		return (horizontal, vertical);
	}

	protected Vector2Int GetInputVector()
	{
		var (horizontal, vertical) = GetInput();
		return new Vector2Int(horizontal, vertical);
	}
}