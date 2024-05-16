using UnityEngine;
using UnityEngine.Tilemaps;

public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
{
	public string Name;
	public string Description;
	public Tile Tile;
	public int Amount;
}