using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="Resource_", menuName ="Scriptable Object/Resource")]
public class Resource : ScriptableObject
{
	public string Name;
	public string Description;
	public Tile Tile;
	public int Amount;
}