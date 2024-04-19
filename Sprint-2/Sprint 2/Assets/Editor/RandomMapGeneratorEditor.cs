using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(AbstractMapGenerator), true)]
public class RandomMapGeneratorEditor : Editor
{
	AbstractMapGenerator Generator;

	public void Awake()
	{
		Generator = (AbstractMapGenerator)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if(GUILayout.Button("Create Dungeon"))
			Generator.Generate();
	}
}
