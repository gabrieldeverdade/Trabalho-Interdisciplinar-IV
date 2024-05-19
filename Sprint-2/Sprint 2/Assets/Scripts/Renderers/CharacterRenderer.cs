using UnityEngine;

public class CharacterRenderer: MonoBehaviour
{
	public void RenderOnTile(BaseTile overlayTyle)
	{
		var character = GetComponent<Character>();
		character.transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y + 0.00002f, overlayTyle.transform.position.z);
		character.GetComponentInChildren<SpriteRenderer>().transform.position = new Vector3(overlayTyle.transform.position.x, overlayTyle.transform.position.y + 0.23f + 0.00002f, overlayTyle.transform.position.z);
		character.GetComponentInChildren<SpriteRenderer>().sortingOrder = overlayTyle.GetComponent<SpriteRenderer>().sortingOrder;

		character.ActiveTile = overlayTyle;
	}
	public void SetStartPosition(int x, int y)
		=> RenderOnTile(MapManager.Instance.Map[new Vector2Int(x, y)]);
}