using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController: InputBaseController
{
	protected bool ShowPath = false;
	protected List<BaseTile> Path = new List<BaseTile>();

	protected List<BaseTile> FindPathBetween(Character enemy, Character destination)
	{
		//Debug.Log($"TRY FOUND PATH TO {destination}");
		if (enemy != null && enemy.ActiveTile != null && destination != null && destination.ActiveTile != null)
		{
			BeforeUpdatePath(Path);
			//Debug.Log("FOUND PATH");
			Path = new PathFinder().Find(enemy, destination, 0);
			AfterUpdatePath(Path);
		}
		return Path;
	}

	protected virtual void BeforeUpdatePath(List<BaseTile> path)
	{
		if (ShowPath)
			foreach (var tile in Path)
				tile.HideTile();
	}

	protected virtual void AfterUpdatePath(List<BaseTile> path)
	{
		if (ShowPath)
			foreach (var tile in Path)
				tile.ShowTile();
	}
}