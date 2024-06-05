using UnityEngine;

public static class DirectionManager
{
	public static DirectionInfo GetDirection(int x, int y)
	{
		var direction = GetDirectionByCoordenates(x, y);
		return new DirectionInfo
		{
			Direction = direction,
			Direction2D = GetDirection(direction)
		};
	}

	public static Vector2 GetDirection(Direction direction) => direction switch
	{
		Direction.N => new Vector2(0.00f, 0.50f),
		Direction.NE => new Vector2(0.50f, 0.25f),
		Direction.E => new Vector2(1.00f, 0.00f),
		Direction.SE => new Vector2(0.50f, -0.25f),
		Direction.S => new Vector2(0.00f, -0.50f),
		Direction.SW => new Vector2(-0.50f, -0.25f),
		Direction.W => new Vector2(-1.00f, 0.00f),
		Direction.NW => new Vector2(-0.50f, 0.25f),
		Direction.None => Vector2.zero,
		_ => Vector2.zero
	};

	public static Direction GetDirectionByCoordenates(int x, int y) => (x, y) switch
	{
		(0, 1) => Direction.N,
		(-1, 1) => Direction.NW,
		(1, 1) => Direction.NE,
		(0, -1) => Direction.S,
		(1, -1) => Direction.SE,
		(-1, -1) => Direction.SW,
		(1, 0) => Direction.E,
		(-1, 0) => Direction.W,
		_ => Direction.None
	};

	public static string GetWalkAnimation(Direction direction) 
		=> $"Walk{GetDirectionName(direction)}";

	public static string GetIdleAnimation(Direction direction) 
		=> $"Idle{GetDirectionName(direction)}";
	
	public static string GetDirectionName(Direction direction) 
		=> direction switch
		{
			Direction.N => "Up",
			Direction.NW => "UpLeft",
			Direction.NE => "UpRight",
			Direction.S => "Bottom",
			Direction.SE => "BottomRight",
			Direction.SW => "BottomLeft",
			Direction.E => "Right",
			Direction.W => "Left",
			_ => "Bottom"
		};
}
