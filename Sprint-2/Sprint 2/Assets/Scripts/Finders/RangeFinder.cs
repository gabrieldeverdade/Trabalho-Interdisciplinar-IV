using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{
	public List<BaseTile> GetTilesInRange(BaseTile start, int range)
	{
		var inRangeTiles = new List<BaseTile>();
		int stepCount = 0;
		inRangeTiles.Add(start);

		var tileForPreviousStep = new List<BaseTile> { start };

		while(stepCount < range)
		{
			var surroundingTiles = new List<BaseTile>();

			foreach(var item in tileForPreviousStep) 
				surroundingTiles.AddRange(item.GetNeightbourTiles(new List<BaseTile>()));

			inRangeTiles.AddRange(surroundingTiles);
			tileForPreviousStep = surroundingTiles.Distinct().ToList();
			stepCount++;
		}

		return inRangeTiles.Distinct().ToList();
	}
}
