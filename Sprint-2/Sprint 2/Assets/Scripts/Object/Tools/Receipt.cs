using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="Resource_", menuName ="Scriptable Object/Receipt")]
public class Receipt : ScriptableObject
{
	public Resource Item;
	public RecipeItem[] Items;
}

[Serializable]
public class RecipeItem
{
	public Tile Tile;
	public int Amount;
}