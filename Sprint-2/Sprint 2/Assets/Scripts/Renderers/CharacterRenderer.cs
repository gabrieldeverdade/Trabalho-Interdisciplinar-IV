using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterRenderer: MonoBehaviour
{
	Dictionary<Direction,BaseTile> OldNeighbours = new();
	public void RenderOnTile(BaseTile overlayTyle)
	{
		var character = GetComponent<Character>();
		//character.transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y + 0.00001f, overlayTyle.transform.position.z);
		//character.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(
		//	character.GetComponentInChildren<SpriteRenderer>().transform.position.x, 
		//	character.GetComponentInChildren<SpriteRenderer>().transform.position.y, 
		//	overlayTyle.transform.position.z+ (overlayTyle.IsBlocked ? 1 : 0)
		//);

		var behindUnalterable = new List<Direction> { Direction.N, Direction.NE, Direction.E };
		var behindList = new List<Direction> { Direction.NW, Direction.N, Direction.NE, Direction.E, Direction.SE};
		var frontList = new List<Direction> { Direction.W, Direction.SW, Direction.S };

		var behindNeighbours = overlayTyle.Neighbours.Where(c => behindList.Contains(c.Key) && !c.Value.Walkable);
		var frontNeighbours = overlayTyle.Neighbours.Where(c => frontList.Contains(c.Key) && !c.Value.Walkable);

		foreach (var tile in OldNeighbours.Where(c => !c.Value.Walkable))
		{
			var isBehind = behindList.Contains(tile.Key);
			var isFront = frontList.Contains(tile.Key);
			//var isCorner = GetCorners(tile.Key, OldNeighbours);

			var order = 1;
			//if (isFront)
			//	order = 5;

			//else if (tile.Key == Direction.NW && isCorner.NWLeft)
			//	order = 1;
			//else if (tile.Key == Direction.NW && (isCorner.NWBottom || isCorner.NWBottomLeft))
			//	order = 5;

			//else if (tile.Key == Direction.SE && isCorner.SETop)
			//	order = 1;
			//else if (tile.Key == Direction.SE && (isCorner.SERight || isCorner.SETopRight))
			//	order = 5;

			tile.Value.GetComponent<SpriteRenderer>().sortingOrder = order;
			tile.Value.SetText("");
		}

		foreach (var tile in behindNeighbours)
		{
			//var isCorner = GetCorners(tile.Key, overlayTyle.Neighbours);
			var order = 1;

			//if(tile.Key == Direction.NW && isCorner.NWLeft)
			//	order = 1;
			//else if (tile.Key == Direction.NW && isCorner.NWBottom)
			//	order = 5;

			//if (tile.Key == Direction.SE && isCorner.SETop)
			//	order = 1;
			//else if (tile.Key == Direction.SE && isCorner.SERight)
			//	order = 5;

			tile.Value.GetComponent<SpriteRenderer>().sortingOrder = order;
			//tile.Value.SetText(tile.Key.ToString());
		}
		foreach (var tile in frontNeighbours)
		{
			tile.Value.GetComponent<SpriteRenderer>().sortingOrder = 5;
			//tile.Value.SetText(tile.Key.ToString());
		}

		character.ActiveTile = overlayTyle;
		OldNeighbours = overlayTyle.Neighbours;
	}
	public void SetStartPosition(int x, int y)
		=> RenderOnTile(MapManager.Instance.Map[new Vector2Int(x, y)]);

	Corners GetCorners(Direction direction, Dictionary<Direction, BaseTile> items)
	{
		var NWLeft= direction == Direction.NW && !items[Direction.N].Walkable && items[Direction.W].Walkable;
		var NWBottom= direction == Direction.NW && items[Direction.N].Walkable && !items[Direction.W].Walkable;
		var NWBottomLeft = direction == Direction.NW && !items[Direction.N].Walkable && !items[Direction.W].Walkable;

		var NETop = direction == Direction.NE && items[Direction.N].Walkable && !items[Direction.E].Walkable;
		var NELeft = direction == Direction.NE && !items[Direction.N].Walkable && items[Direction.E].Walkable;
		var NETopLeft = direction == Direction.NE && !items[Direction.N].Walkable && !items[Direction.W].Walkable;

		var SETop = direction == Direction.SE && items[Direction.S].Walkable && !items[Direction.E].Walkable;
		var SERight = direction == Direction.SE && !items[Direction.S].Walkable && items[Direction.E].Walkable;
		var SETopRight= direction == Direction.SE && !items[Direction.N].Walkable && !items[Direction.W].Walkable;
		
		var SWBottom = direction == Direction.SW && items[Direction.S].Walkable && !items[Direction.W].Walkable;
		var SWRight = direction == Direction.SW && !items[Direction.S].Walkable && items[Direction.W].Walkable;
		var SWBottomRight= direction == Direction.SW && !items[Direction.S].Walkable && !items[Direction.W].Walkable;

		return new Corners
		{
			NWBottom = NWBottom,
			NWLeft = NWLeft,
			NWBottomLeft  = NWBottomLeft,
			NETop  = NETop,
			NELeft  = NELeft,
			NETopLeft  = NETopLeft,
			SETop  = SETop,
			SERight  = SERight,
			SETopRight = SETopRight,
			SWBottom  = SWBottom,
			SWRight  = SWRight,
			SWBottomRight = SWBottomRight,
		};
	}
}

class Corners
{
	public bool NWBottom;
	public bool NWLeft;
	public bool NWBottomLeft;
	public bool NETop;
	public bool NELeft;
	public bool NETopLeft;
	public bool SETop;
	public bool SERight;
	public bool SETopRight;
	public bool SWBottom;
	public bool SWRight;
	public bool SWBottomRight;
}