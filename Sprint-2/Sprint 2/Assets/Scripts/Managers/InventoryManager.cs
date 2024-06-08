using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] Slot[] Slots;
	[SerializeField] Character Character;

	void Start()
	{
		Slots = GetComponentsInChildren<Slot>();
		foreach (var slot in Slots)
			slot.Hide();
	}

	void Update()
	{
		if (Slots != null && Slots.Any() && Character.BaseInventory != null && Character.BaseInventory.ResourcesInBag != null)
		{
			var itemsInBag = Character.BaseInventory.ResourcesInBag;

			for(var i = 0; i < Slots.Length; i++)
			{
				if (itemsInBag.ElementAtOrDefault(i) == null) break;

				Slots[i].Resource = itemsInBag[i].Resource;
				Slots[i].Quantity = itemsInBag[i].Amount;
			}
		}

		foreach(var slot in Slots)
			slot.Draw();
	}

}
