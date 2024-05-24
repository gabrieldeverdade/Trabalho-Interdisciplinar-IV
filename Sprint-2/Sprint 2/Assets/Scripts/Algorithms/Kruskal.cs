using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Edge : IComparable<Edge>
{
	public int Source { get; }
	public int Destination { get; }
	public int Weight { get; }

	public Edge(int source, int destination, int weight)
	{
		Source = source;
		Destination = destination;
		Weight = weight;
	}

	public int CompareTo(Edge other)
	{
		return Weight.CompareTo(other.Weight);
	}
}

class UnionFind
{
	private int[] parent;
	private int[] rank;

	public UnionFind(int size)
	{
		parent = new int[size];
		rank = new int[size];
		for (int i = 0; i < size; i++)
		{
			parent[i] = i;
			rank[i] = 0;
		}
	}

	public int Find(int u)
	{
		if (parent[u] != u)
		{
			parent[u] = Find(parent[u]); // Path compression
		}
		return parent[u];
	}

	public void Union(int u, int v)
	{
		int rootU = Find(u);
		int rootV = Find(v);

		if (rootU != rootV)
		{
			if (rank[rootU] > rank[rootV])
			{
				parent[rootV] = rootU;
			}
			else if (rank[rootU] < rank[rootV])
			{
				parent[rootU] = rootV;
			}
			else
			{
				parent[rootV] = rootU;
				rank[rootU]++;
			}
		}
	}
}

class KruskalMST
{
	List<Edge> Kruskal(int vertices, List<Edge> edges)
	{
		List<Edge> result = new List<Edge>();
		edges.Sort();
		UnionFind unionFind = new UnionFind(vertices);

		foreach (Edge edge in edges)
		{
			int rootSource = unionFind.Find(edge.Source);
			int rootDestination = unionFind.Find(edge.Destination);

			if (rootSource != rootDestination)
			{
				result.Add(edge);
				unionFind.Union(rootSource, rootDestination);
			}
		}

		return result;
	}

	public List<(BaseTile start, BaseTile end)> Run(List<(BaseTile start, BaseTile end)> tiles)
	{
		var edges = new List<Edge>();
		var itemsDict = new Dictionary<BaseTile, int>();
		var i = 0;

		var mappedStart = 0;
		var mappedEnd = 0;

		foreach(var tile in tiles)
		{
			var weight = GetManhattanDistance(tile.start, tile.end);

			if (!itemsDict.TryGetValue(tile.start, out mappedStart))
				itemsDict.Add(tile.start, i++);

			if (!itemsDict.TryGetValue(tile.end, out mappedEnd))
				itemsDict.Add(tile.end, i++);

			edges.Add(new Edge(mappedStart, mappedEnd, (int)weight));
		}

		int vertices = itemsDict.Count;
		List<Edge> mst = Kruskal(vertices, edges);

		var transposeDict = itemsDict.ToDictionary(d => d.Value, d => d.Key);

		return mst.Select(m => (transposeDict[m.Source], transposeDict[m.Destination])).ToList();
	}
	float GetManhattanDistance(BaseTile start, BaseTile neighbor)
		=> Mathf.Abs(start.GridLocation.x - neighbor.GridLocation.x) + Mathf.Abs(start.GridLocation.y - neighbor.GridLocation.y);
}
