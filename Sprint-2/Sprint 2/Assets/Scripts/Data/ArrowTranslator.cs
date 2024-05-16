using UnityEngine;

public partial class ArrowTranslator
{
	public ArrowDirection Translate(BaseTile previous, BaseTile current, BaseTile next)
	{
		bool isFinal = next == null;

		var pastDirection = previous != null ? current.GridLocation2D - previous.GridLocation2D : Vector2Int.zero;
		var nextDirection =  next != null ? next.GridLocation2D - current.GridLocation2D : Vector2Int.zero;

		var direction = pastDirection != nextDirection ? pastDirection + nextDirection : nextDirection;

		if (direction == new Vector2Int(0, 1) && !isFinal) return ArrowDirection.Up;
		if (direction == new Vector2Int(0,-1) && !isFinal) return ArrowDirection.Down;
		if (direction == new Vector2Int(1, 0) && !isFinal) return ArrowDirection.Right;
		if (direction == new Vector2Int(-1,0) && !isFinal) return ArrowDirection.Left;

		if (direction == new Vector2Int(1, 1) && pastDirection.y < nextDirection.y) return ArrowDirection.BottomLeft;
		if (direction == new Vector2Int(1, 1) && pastDirection.y >= nextDirection.y) return ArrowDirection.TopRight;

		if (direction == new Vector2Int(-1, 1) && pastDirection.y < nextDirection.y) return ArrowDirection.BottomRight;
		if (direction == new Vector2Int(-1, 1) && pastDirection.y >= nextDirection.y) return ArrowDirection.TopLeft;

		if (direction == new Vector2Int(1, -1) && pastDirection.y > nextDirection.y) return ArrowDirection.TopLeft;
		if (direction == new Vector2Int(1, -1) && pastDirection.y <= nextDirection.y) return ArrowDirection.BottomRight;

		if (direction == new Vector2Int(-1, -1) && pastDirection.y > nextDirection.y) return ArrowDirection.TopRight;
		if (direction == new Vector2Int(-1, -1) && pastDirection.y <= nextDirection.y) return ArrowDirection.BottomLeft;

		if (direction == new Vector2Int(0, 1) && isFinal) return ArrowDirection.UpFinished;
		if (direction == new Vector2Int(0, -1) && isFinal) return ArrowDirection.DownFinished;
		if (direction == new Vector2Int(1, 0) && isFinal) return ArrowDirection.RightFinished;
		if (direction == new Vector2Int(-1, 0) && isFinal) return ArrowDirection.LeftFinished;


		return ArrowDirection.None;
	}
}
