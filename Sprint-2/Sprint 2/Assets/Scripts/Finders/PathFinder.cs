using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder
{
	public int JumpHeight = 1;

	public List<BaseTile> Find(Character character, Character destination, int range = 0)
		=> Find(character, character.ActiveTile, destination.ActiveTile, range);

	public List<BaseTile> Find(Character character, BaseTile start, BaseTile end, int range = 0)
	{
		List<BaseTile> open = new List<BaseTile>();
		List<BaseTile> closed = new List<BaseTile>();

		var availableTiles = range > 0 ? new RangeFinder().GetTilesInRange(start, range) : new List<BaseTile>();
		open.Add(start);
		while (open.Count > 0)
		{
			BaseTile current = open.OrderBy(x => x.F).First();

			open.Remove(current);
			closed.Add(current);

			if (current == end) return GetFinishedList(start, end);

			var validMoves = new List<Direction> { Direction.N, Direction.S, Direction.E, Direction.W };
			foreach (var neighbourKeyValue in current.Neighbours.Where(c => validMoves.Contains(c.Key)))
			{
				var neighbour = neighbourKeyValue.Value;

				var flyable = character.CanFly && neighbour.Flyable;
				var walkable = neighbour.Walkable;
				if (!(flyable || walkable) || closed.Contains(neighbour) || Mathf.Abs(current.GridLocation.z - neighbour.GridLocation.z) > JumpHeight )
					continue;

				//Debug.Log("CALCULATING  G");
				neighbour.G = GetManhattanDistance(start, neighbour);
				//Debug.Log("CALCULATING  H");
				neighbour.H = GetManhattanDistance(end, neighbour);
				neighbour.Previous = current;
				 
				if(!open.Contains(neighbour))
					open.Add(neighbour);
			}
		}

		return new List<BaseTile>();
	}

	List<BaseTile> GetFinishedList(BaseTile start, BaseTile end)
	{
		var finisheds = new List<BaseTile>();
		var current = end;
		while(current != start)
		{
			finisheds.Add(current);
			current = current.Previous;
		}
		finisheds.Reverse();
		return finisheds;
	}

	int GetManhattanDistance(BaseTile start, BaseTile neighbor)
	{
		//Debug.Log($"{start} / {neighbor}");
		return Mathf.Abs(start.GridLocation.x - neighbor.GridLocation.x) + Mathf.Abs(start.GridLocation.y - neighbor.GridLocation.y);
	}

}
