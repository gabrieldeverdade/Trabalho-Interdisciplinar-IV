using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
	public Resource Resource;
	public int Quantity;

	public void ShowSlot()
	{
		var spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
		if (spriteRenderer != null) spriteRenderer.enabled = true;
	}

	public void HideSlot()
	{
		var spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
		if (spriteRenderer != null) spriteRenderer.enabled = false;
	}

	public void Draw()
	{
		var text = GetComponentInChildren<Text>();
		if (text != null)
		{
			text.enabled = true;
			text.text = Quantity.ToString();
		}

		if (Resource != null)
		{
			var spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
			//Debug.Log($"{spriteRenderer.name}");
			spriteRenderer.enabled = true;
			spriteRenderer.sprite = Resource.Tile.sprite;
		} else
		{
			var spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
			//Debug.Log($"{spriteRenderer.name}");
			spriteRenderer.enabled = true;
			spriteRenderer.sprite = MapManager.Instance.WIPTile.sprite;
		}
	}

	public void Hide()
	{
		var text = GetComponentInChildren<Text>();
		if(text != null)  text.enabled = false;

		var spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
		if(spriteRenderer != null) spriteRenderer.enabled = false;

	}
}