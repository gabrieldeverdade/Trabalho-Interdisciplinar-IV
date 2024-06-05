using UnityEngine;

public class DirectionInfo
{
	public Direction Direction { get; set; }
	public Vector2 Direction2D { get; set; }

	public Vector2Int Direction2DInt => new Vector2Int((int)Direction2D.x, (int)Direction2D.y);
	public Vector3 Direction3D => new Vector3(Direction2D.x, Direction2D.y, 0);
	public Vector3Int Direction3DInt => new Vector3Int((int)Direction2D.x, (int)Direction2D.y, 0);
}