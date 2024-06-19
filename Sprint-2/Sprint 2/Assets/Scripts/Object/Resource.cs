using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName="Resource_", menuName ="Scriptable Object/Resource")]
public class Resource : ScriptableObject
{
	public string Name;
	public string Description;
	public Tile Tile;
	public int Amount;

	[Header("Informaçoes de armas")]
	public bool CanHitEnemies= false;
	public int Attack = 10;

	[Header("Informaçoes de coletores de recurso")]
	public bool CanGetResources = false;
	public List<Resource> GettableResources = new List<Resource>();
}