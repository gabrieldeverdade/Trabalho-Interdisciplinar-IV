using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManager : MonoBehaviour
{
	[SerializeField] Slot[] Slots;
	[SerializeField] Receipt[] Receipts;
	[SerializeField] Vector3? OpenAt;

	void Start()
	{
		Slots = GetComponentsInChildren<Slot>();
		foreach (var slot in Slots)
		{
			slot.Draw();
			slot.HideSlot();
		}
	}

	public void OpenOn(Vector3 position) => OpenAt = position;
	public void Close() => OpenAt = null;

	public Resource CreateItem(int index, List<ResourceInBag> bag)
	{
		var receipts = Receipts[index];
		if(receipts == null) return null;

		foreach(var receiptItem in receipts.Items)
		{
			var foundTile = bag.FirstOrDefault(c => c.Resource.Tile == receiptItem.Tile);

			if (foundTile == null) return null;

			if (foundTile.Amount > receiptItem.Amount)
				foundTile.Amount -= receiptItem.Amount;
			else
				return null;
		}
		return receipts.Item;
	}

	void Update()
	{
		if (OpenAt != null)
		{
			gameObject.transform.position = OpenAt.Value;
			foreach(var slot in Slots)
			{
				slot.ShowSlot();
				slot.Draw();
			}
		}
		else
		{
			foreach (var slot in Slots)
			{
				slot.Hide();
				slot.HideSlot();
			}
		}
	}

}
