using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] Slot[] Slots;
	[SerializeField] Character Character;
	[SerializeField] CharacterManager CharacterManager;


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
				if (itemsInBag.ElementAtOrDefault(i) != null)
				{
					Slots[i].Resource = itemsInBag[i].Resource;
					Slots[i].Quantity = itemsInBag[i].Amount;
					Slots[i].Selected = CharacterManager.SelectedWeaponIndex == i;
				} 
				else
				{
					Slots[i].Resource = null;
					Slots[i].Quantity = 0;
					Slots[i].Selected = false;
					Slots[i].Hide();
				}

			}
		}

		foreach (var slot in Slots)
			slot.Draw();
	}

}
