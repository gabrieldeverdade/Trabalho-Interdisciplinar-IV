using System;
using System.Numerics;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public class Attribute
{
	public int Current;
	public int Start;
	public float Percentage => (float)Current / (float)Start;

	public Attribute(int start)
	{
		Current = Start = start;
	}

	public bool Remove(int amount)
	{
		Current -= amount;
		return Current <= 0;
	}

	public bool Add(int amount)
	{
		Current += amount;
		Current = Math.Min(Current, Start);
		return Current > 0;
	}

}