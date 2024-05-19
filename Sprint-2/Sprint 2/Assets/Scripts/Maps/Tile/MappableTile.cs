using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="MappableTile_", menuName ="Scriptable Object/Mappable Tile")]
public class MappableTile : ScriptableObject
{
	public string Name;
	public Tile Tile;

	public bool Walkable;
	public bool Flyable;
	public bool Resourceable;
	public bool OnTopOfGround;

	public List<Tile> ValidTiles;
}
