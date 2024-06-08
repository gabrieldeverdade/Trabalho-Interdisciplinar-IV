using UnityEngine;

public class TileBounds
{
	public Vector3 Left;
	public Vector3 Right;
	public Vector3 Up;
	public Vector3 Down;

	public bool HasInside(Vector3 character)
		=> IsPointInDiamond(character, new Vector2[] { Left, Up, Right, Down });

	bool IsPointInTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
	{
		double sign(Vector2 p1, Vector2 p2, Vector2 p3) => (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
		bool b1 = sign(P, A, B) < 0.0, b2 = sign(P, B, C) < 0.0, b3 = sign(P, C, A) < 0.0;
		return ((b1 == b2) && (b2 == b3));
	}

	bool IsPointInDiamond(Vector2 point, Vector2[] diamondVertices)
		=> IsPointInTriangle(point, diamondVertices[0], diamondVertices[1], diamondVertices[2]) 
		|| IsPointInTriangle(point, diamondVertices[0], diamondVertices[2], diamondVertices[3]);
}