using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class Position
{
	public Vector3 Left;
	public Vector3 Right;
	public Vector3 Up;
	public Vector3 Down;

	public bool HasInside(Vector3 character)
	{
		return IsPointInDiamond(character, new Vector2[] { Left, Up , Right, Down});
	}

	static bool IsPointInTriangle(Vector2 P, Vector2 A, Vector2 B, Vector2 C)
	{
		double sign(Vector2 p1, Vector2 p2, Vector2 p3) => (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
		bool b1 = sign(P, A, B) < 0.0, b2 = sign(P, B, C) < 0.0, b3 = sign(P, C, A) < 0.0;
		return ((b1 == b2) && (b2 == b3));
	}

	static bool IsPointInDiamond(Vector2 point, Vector2[] diamondVertices)
	{
		return IsPointInTriangle(point, diamondVertices[0], diamondVertices[1], diamondVertices[2]) ||
				 IsPointInTriangle(point, diamondVertices[0], diamondVertices[2], diamondVertices[3]);
	}
}

public class KeyboardController : BaseController
{
	public int X;
	public int Y;

	public int JumpHeight = 2;
	public float Speed = 10;
	public Direction LatestDirection = Direction.S;
	public Direction CurrentDirection = Direction.S;
	public int CurrentZ = 0;
	public bool IsClimbing = false;
	public float zPositionChanger = 0.25f;
	public Position POSITION;
	public BaseTile LatestPosition;
	public InsideTileMover InsideTileMover;

	[SerializeField] Character Character;
	[SerializeField] int StartPositionX;
	[SerializeField] int StartPositionY;
	PlayerAnimation PlayerAnimation;

	private void Start()
	{
		PlayerAnimation = Character.GetComponent<PlayerAnimation>();
		InsideTileMover = new InsideTileMover();
	}

	private void Update()
	{
		if (Character.ActiveTile == null)
		{
			Character.ActiveTile = MapManager.Instance.Map[new Vector2Int(StartPositionX, StartPositionY)];
			PlayerAnimation.Play("IdleBottom");
		}
	
		var horizontal = Input.GetAxisRaw("Horizontal") > 0 ? 1 : Input.GetAxisRaw("Horizontal") < 0 ? - 1 : 0;
		var vertical = Input.GetAxisRaw("Vertical") > 0 ? 1 : Input.GetAxisRaw("Vertical") < 0 ? - 1 : 0;

		Walk(new Vector2Int(horizontal , vertical));
	}

	Direction GetDirectionDegrees(Vector2 direction)
	{
		float step = 360 / 8;
		float offset = step / 2;
		float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

		angle += offset;
		if (angle < 0) angle += 360;

		float stepCount = angle / step;

		return (Direction)Enum.ToObject(typeof(Direction), Mathf.FloorToInt(stepCount));
	}

	void ColorWithTransparent(BaseTile tile)
		=> tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);


	void ColorDirectionWithBlue(BaseTile tile)
		=> tile.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);

	void ColorCurrentTileWithWhite(BaseTile tile)
		=> tile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

	void Walk(Vector2Int direction)
	{
		if (Character == null || Character.ActiveTile == null) return;

		ClearLatestDrawn();
		ColorCurrentTileWithWhite(Character.ActiveTile);


		if (InsideTileMover.Move(direction, Character)) return;

		MapManager.Instance.Map[new Vector2Int(X, Y)].GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 0);

		if (Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.Equals)) X++;
		if (Input.GetKey(KeyCode.X) && Input.GetKeyDown(KeyCode.Minus)) X--;

		if (Input.GetKey(KeyCode.Y) && Input.GetKeyDown(KeyCode.Equals)) Y++;
		if (Input.GetKey(KeyCode.Y) && Input.GetKeyDown(KeyCode.Minus)) Y--;


		if(MapManager.Instance.Map != null)
			MapManager.Instance.Map[new Vector2Int(X, Y)].GetComponent<SpriteRenderer>().color = new Color(1, 0, 1, 1);


		//var directionEnum = GetDirection(direction.x, direction.y);
		//var directionVector = GetDirection(directionEnum);
		//var directionVector3 = new Vector3(directionVector.x, directionVector.y, 0);

		//var possibleNewPosition = new Vector2(Character.transform.position.x, Character.transform.position.y) + directionVector;
		//var worldPositonTransposed = new Vector3(possibleNewPosition.x, possibleNewPosition.y, 0);
		//var worldPosition = MapBuilder.Instance.Tilemap.WorldToCell(worldPositonTransposed);
		//var futurePosition = MapManager.Instance.Map[new Vector2Int(worldPosition.x, worldPosition.y)];
		//LatestPosition = futurePosition;

		//ColorDirectionWithBlue(LatestPosition);

		//Debug.Log($"CHARACTER: {Character.transform.position}");
		//Debug.Log($"WORLD: {MapBuilder.Instance.Tilemap.WorldToCell(worldPositonTransposed)}");

		//var charPosition = Character.ActiveTile.transform.position;
		//POSITION = new Pos
		//{
		//	Left	= new Vector3(charPosition.x - 0.5f, charPosition.y, 0),
		//	Right = new Vector3(charPosition.x + 0.5f, charPosition.y, 0),
		//	Up		= new Vector3(charPosition.x, charPosition.y + 0.25f, 0),
		//	Down	= new Vector3(charPosition.x, charPosition.y - 0.25f, 0),
		//};

		//if (IsInside(Character.transform.position))
		//{
		//	Character.GetComponent<Rigidbody2D>().velocity = directionVector3;
		//}
		//else
		//{
		//	var velocity = Character.GetComponent<Rigidbody2D>().velocity;
		//	var velocity3d = new Vector3(velocity.x, velocity.y, 0) * 0.1f;

		//	Character.GetComponent<Rigidbody2D>().position = Character.GetComponent<Rigidbody2D>().transform.position-velocity3d;
		//	Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		//}

		//Debug.Log($"IS INSIDE TILE: {IsInside(Character.transform.position)}");
		//Debug.Log($"Character Position: {Character.transform.position}");
		//Debug.Log($"X Limits: {POSITION.Left.x} ~ {POSITION.Right.x}");
		//Debug.Log($"Y Limits: {POSITION.Up.y} ~ {POSITION.Down.y}");
		//if (LatestPosition == null || !LatestPosition.Walkable)
		//{
		//	if (Vector2.Distance(Character.transform.position, worldPositonTransposed) < 0.0001f)
		//		Character.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
		//	else
		//		Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		//}
		//else
		//{
		//	if (LatestPosition.Climbable)
		//	{
		//		var zPosition = zPositionChanger * (Character.ActiveTile.Height > LatestPosition.Height ? -1 : 1);
		//		Debug.Log($"CLIMBABLE {zPosition}");
		//		Character.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y + zPosition);
		//	}
		//	else
		//	{
		//		Character.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y);
		//	}
		//}

		//foreach (var neigbour in LatestPosition.Neighbours)
		//{
		//	if (neigbour.Value.GetComponentInChildren<Text>())
		//	{
		//		neigbour.Value.GetComponentInChildren<Text>().text = $"({neigbour.Value.transform.position.x},{neigbour.Value.transform.position.y.ToString("0.####")},{neigbour.Value.transform.position.z})";
		//		neigbour.Value.GetComponentInChildren<Text>().enabled = true;
		//	}

		//	neigbour.Value.GetComponent<SpriteRenderer>().color = new Color(neigbour.Value.Walkable ? 0 : 1, neigbour.Value.Walkable ? 1 : 0, 0, 1);
		//}
		//LatestPosition = futurePosition;

		//foreach (var tile in Character.ActiveTile.Neighbours)
		//{
		//	if (tile.Value.GetComponentInChildren<Text>())
		//		tile.Value.GetComponentInChildren<Text>().enabled = false;

		//	tile.Value.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
		//	Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		//}

		//Character.ActiveTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.0f);
		//if (mapPosition == null || !mapPosition.Walkable)
		//{
		//	if (Vector2.Distance(Character.transform.position, worldPositonTransposed) < 0.0001f)
		//		Character.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y) * Speed;
		//	else
		//		Character.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		//}
		//else
		//{
		//	if(mapPosition.Climbable)
		//	{
		//		var zPosition = zPositionChanger * (Character.ActiveTile.Height > mapPosition.Height ? -1: 1);
		//		Debug.Log($"CLIMBABLE {zPosition}");
		//		Character.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y + zPosition) * Speed;
		//	}
		//	else
		//	{
		//		Character.GetComponent<Rigidbody2D>().velocity = new Vector2(direction.x, direction.y ) * Speed;
		//	}

		//	foreach(var neigbour in mapPosition.Neighbours)
		//	{
		//		if (neigbour.Value.GetComponentInChildren<Text>())
		//		{
		//			neigbour.Value.GetComponentInChildren<Text>().text = $"({neigbour.Value.transform.position.x},{neigbour.Value.transform.position.y.ToString("0.####")},{neigbour.Value.transform.position.z})";
		//			neigbour.Value.GetComponentInChildren<Text>().enabled = true;
		//		}

		//		neigbour.Value.GetComponent<SpriteRenderer>().color = new Color(neigbour.Value.Walkable ? 0 : 1, neigbour.Value.Walkable ? 1 : 0, 0, 1);
		//	}

		//	var tile = mapPosition;
		//	if (tile.GetComponentInChildren<Text>())
		//	{
		//		tile.GetComponentInChildren<Text>().text = $"({tile.transform.position.x},{tile.transform.position.y.ToString("0.####")},{tile.transform.position.z})";
		//		tile.GetComponentInChildren<Text>().enabled = true;
		//	}
		//	Character.GetComponent<CharacterRenderer>().RenderOnTile(mapPosition);
		//	Character.ActiveTile.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
		//}

		var latestDirection = GetDirectionDegrees(direction);

		if ((direction.magnitude / Speed) > 0.001)
			CurrentDirection = latestDirection;

		if ((direction.magnitude / Speed) > 0.5)
			LatestDirection = latestDirection;

		PlayerAnimation.Play(
			direction.magnitude < 0.001
			? DirectionManager.GetIdleAnimation(LatestDirection)
			: DirectionManager.GetWalkAnimation(latestDirection)
		);
	}

	void OnDrawGizmos()
	{
		if (InsideTileMover != null) InsideTileMover.DrawGizmos();
	}

	void ClearLatestDrawn()
	{
		if (LatestPosition != null)
		{
			ColorWithTransparent(LatestPosition);

			foreach (var neigbour in LatestPosition.Neighbours)
			{
				if (neigbour.Value.GetComponentInChildren<Text>())
				{
					neigbour.Value.GetComponentInChildren<Text>().enabled = false;
				}

				neigbour.Value.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
			}
		}

	}
}
