using System;
using System.Numerics;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public class Attribute
{
	public float Current;
	public float Start;
	public float Percentage => (float)Current / (float)Start;

	public Attribute(float start)
	{
		Current = Start = start;
	}

	public bool Remove(float amount)
	{
		Current -= amount;
		return Current <= 0;
	}

	public bool Add(float amount)
	{
		Current += amount;
		Current = Math.Min(Current, Start);
		return Current > 0;
	}

}