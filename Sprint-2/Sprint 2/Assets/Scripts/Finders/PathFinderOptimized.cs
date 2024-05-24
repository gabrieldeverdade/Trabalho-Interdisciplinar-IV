using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinderOptimized
{
	public int JumpHeight = 1;

	public List<BaseTile> Find(BaseTile start, BaseTile end, int range = 0)
	{
		var toSearch = new List<BaseTile>();
		var processed = new List<BaseTile>();
		var currentRange = 0;

		toSearch.Add(start);
		while (toSearch.Count > 0)
		{
			var current = toSearch[0];
			currentRange++;
			foreach (var node in toSearch)
				if (node.F < current.F || node.F == current.F && node.H < current.H)
					current = node;

			processed.Add(current);
			toSearch.Remove(current);

			if (current == end)
			{
				var currentPathTile = end;
				var path = new List<BaseTile>();
				while(currentPathTile != start)
				{
					path.Add(currentPathTile);
					currentPathTile = currentPathTile.Previous;
				}
				path.Reverse();
				return path;
			}

			if(Vector3.Distance(start.transform.position, current.transform.position) > range && range != 0) 
				return new List<BaseTile>();

			foreach (var neighbour in current.Neighbours.Values.Where(c => (!c.IsBlocked && !processed.Contains(c)) || c == end))
			{
				var inSearch = toSearch.Contains(neighbour);
				var costToNeighbour = current.G + GetManhattanDistance(current, neighbour);
				if(neighbour.Walkable && (!inSearch || costToNeighbour < neighbour.G))
				{
					neighbour.G = costToNeighbour;
					neighbour.Previous = current;

					if (!inSearch)
					{
						neighbour.H = GetManhattanDistance(current, end);
						toSearch.Add(neighbour);
					}
				}
			}
		}

		return new List<BaseTile>();
	}

	float GetManhattanDistance(BaseTile start, BaseTile neighbor)
		=> Mathf.Abs(start.GridLocation.x - neighbor.GridLocation.x) + Mathf.Abs(start.GridLocation.y - neighbor.GridLocation.y);
}
