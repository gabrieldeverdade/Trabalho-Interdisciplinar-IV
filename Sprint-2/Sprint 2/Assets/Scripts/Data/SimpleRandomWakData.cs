using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SimpleRandomWalkParameters_", menuName = "PCG/SimpleRandomWalkData")]
public class SimpleRandomWakData : ScriptableObject
{
	[SerializeField] public int Iterations = 10;
	[SerializeField] public int WalkLength = 10;
	[SerializeField] public bool StartRandomlyEachIteration = true;
}
