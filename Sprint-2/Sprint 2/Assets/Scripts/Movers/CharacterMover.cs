using System.IO;
using UnityEngine;

public class CharacterMover: MonoBehaviour
{
	public bool Move(float speed, BaseTile tile)
	{
		var character = GetComponent<Character>();
		var renderer  = GetComponent<CharacterRenderer>();

		var firstPosition = tile.transform.position;
		var step = speed * Time.deltaTime;
		var z = firstPosition.z;

		character.transform.position = Vector2.MoveTowards(character.transform.position, firstPosition, step);
		character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, z);

		if (Vector2.Distance(character.transform.position, firstPosition) < 0.0001f)
		{
			renderer.RenderOnTile(tile);
			tile.HideTile();
			return true;
		}
		return false;
	}
}