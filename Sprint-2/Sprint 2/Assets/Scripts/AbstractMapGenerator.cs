using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMapGenerator : MonoBehaviour
{
   [SerializeField] protected TilemapVisualizer TilemapVisualizer = null;
	[SerializeField] protected Vector2 StartPosition = Vector2.zero;

	public void Generate()
	{
		TilemapVisualizer.Clear();
		RunProceduralGeneration();
	}

	protected abstract void RunProceduralGeneration();
}
