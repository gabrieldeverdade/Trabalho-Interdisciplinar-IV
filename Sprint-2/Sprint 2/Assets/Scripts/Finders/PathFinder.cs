using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder 
{
	public int JumpHeight = 1;
	public bool IsPlayer = false;
	public RangeFinder RangeFinder;

	public PathFinder(bool isPlayer)
	{
		IsPlayer = isPlayer;
		RangeFinder = new RangeFinder();
	}	

	public List<BaseTile> Find(BaseTile start, BaseTile end, int range = 0)
	{
		List<BaseTile> open = new List<BaseTile>();
		List<BaseTile> closed = new List<BaseTile>();

		var availableTiles = range > 0 ? RangeFinder.GetTilesInRange(start, range) : new List<BaseTile>();
		open.Add(start);
		while (open.Count > 0)
		{
			BaseTile current = open.OrderBy(x => x.F).First();

			open.Remove(current);
			closed.Add(current);

			if (current == end) return GetFinishedList(start, end);

			var neighbours = current.GetNeightbourTiles(availableTiles);

			foreach(var neighbour in neighbours)
			{
				if (neighbour != end && (
						neighbour.IsBlocked 
						|| closed.Contains(neighbour) 
						|| Mathf.Abs(current.GridLocation.z - neighbour.GridLocation.z) > JumpHeight 
						|| (!IsPlayer && !neighbour.CanUsePath)
					))
					continue;

				neighbour.G = GetManhattanDistance(start, neighbour);
				neighbour.H = GetManhattanDistance(neighbour, start);
				neighbour.Previous = current;
				 
				if(!open.Contains(neighbour))
					open.Add(neighbour);

			}
		}

		return new List<BaseTile>();
	}

	private List<BaseTile> GetFinishedList(BaseTile start, BaseTile end)
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

	private int GetManhattanDistance(BaseTile start, BaseTile neighbor)
		=> Mathf.Abs(start.GridLocation.x - neighbor.GridLocation.x) + Mathf.Abs(start.GridLocation.y - neighbor.GridLocation.y);

}
