using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class Slot : MonoBehaviour
{
	public Resource Resource;
	public int Quantity;
	public bool Selected;

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

			spriteRenderer.enabled = true;
			spriteRenderer.sprite = Resource.Tile.sprite;

			var spriteRenderer2 = GetComponentsInChildren<SpriteRenderer>().ElementAtOrDefault(2);
			if(spriteRenderer2 != null)  spriteRenderer2.enabled = true;
		}
		else
		{
			var spriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
			spriteRenderer.enabled = true;
		}

		GetComponentsInChildren<SpriteRenderer>()[0].color = Selected ? Color.red : Color.white;
	}

	public void Hide()
	{
		var text = GetComponentInChildren<Text>();
		if(text != null)  text.enabled = false;

		var spriteRenderer = GetComponentsInChildren<SpriteRenderer>().ElementAtOrDefault(1);
		if(spriteRenderer != null) spriteRenderer.enabled = false;

		var spriteRenderer2 = GetComponentsInChildren<SpriteRenderer>().ElementAtOrDefault(2);
		if (spriteRenderer2 != null) spriteRenderer2.enabled = false;
	}
}