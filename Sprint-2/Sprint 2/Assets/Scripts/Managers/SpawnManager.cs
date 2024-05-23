using System.Collections.Generic;
using System.Linq;

public class SpawnManager: SingletonMonoBehaviour<SpawnManager>
{
	public SpawnArea[] SpawnAreas;

	public List<BaseTile> HasSeenAnyArea(BaseTile tile, int range = 0)
	{
		var pathFinder = new PathFinderOptimized();
		var rangeFinder = new RangeFinder();

		var rangePath = rangeFinder.GetTilesInRange(tile, range);
		var resource = rangePath.FirstOrDefault(c => c.Resourceable);

		if(resource == null) return new List<BaseTile>();

		var path = pathFinder.Find(tile, resource, range);

		if (path.Count > 0)
			return path;

		return new List<BaseTile>();
	}
}