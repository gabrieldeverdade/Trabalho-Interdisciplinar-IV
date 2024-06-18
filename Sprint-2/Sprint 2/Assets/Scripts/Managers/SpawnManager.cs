using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager: SingletonMonoBehaviour<SpawnManager>
{
	public SpawnArea[] SpawnAreas;

	public List<List<BaseTile>> KruskalsPath = new();

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

	//private void Update()
	//{
	//	if (Input.GetKeyDown("r"))
	//	{
	//		KruskalsPath.Clear();
	//		var edges = SpawnAreas.SelectMany(c => c.ResourcesPath.Select(d => (c.SpawnTile, d.Value.Last()))).ToList();
	//		var kruskal = new KruskalMST().Run(edges);

	//		if (kruskal != null)
	//		{
	//			var pathFinder = new PathFinderOptimized();
	//			foreach (var krusk in kruskal)
	//			{
	//				var path = pathFinder.Find(krusk.start, krusk.end);
	//				KruskalsPath.Add(path);
	//			}
	//		}
	//	}
	//}

	private void OnDrawGizmos()
	{
		foreach(var path in KruskalsPath)
		{
			for (var i = 0; i < path.Count - 2; i++)
			{
				var current = path[i];
				var next = path[i + 1];
				Gizmos.color = new Color(
					1,
					0,
					0
				);
				Gizmos.DrawLine(current.transform.position, next.transform.position);
			}
		}
	}
}