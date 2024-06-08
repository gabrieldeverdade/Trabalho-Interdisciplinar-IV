using UnityEngine;

public class TileSpecificMover
{
	public bool Move(Character character, float speed, BaseTile tile)
	{
		var renderer  = character.GetComponent<CharacterRenderer>();

		var firstPosition = tile.transform.position;
		var step = speed * Time.deltaTime;
		var z = firstPosition.z;

		character.transform.position = Vector2.MoveTowards(character.transform.position, firstPosition, step);
		character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, z);

		if (Vector2.Distance(character.transform.position, firstPosition) < 0.5f)
		{
			renderer.RenderOnTile(tile);
			return true;
		}
		return false;
	}
}