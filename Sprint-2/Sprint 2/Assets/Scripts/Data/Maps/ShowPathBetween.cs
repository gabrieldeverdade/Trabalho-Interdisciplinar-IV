using System.Collections.Generic;
using UnityEngine;

public class ShowPathBetween: MonoBehaviour
{
	public float Speed = 5;

	[SerializeField] bool ShowPath = false;

	List<OverlayTile> Path = new();
	PathFinder PathFinder;

	private void Start()
	{
	}

	// Update is called once per frame
	void LateUpdate()
	{
		
	}
}
