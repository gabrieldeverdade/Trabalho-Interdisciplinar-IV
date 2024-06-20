using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
	[SerializeField] BaseTile ClosestResource;
	[SerializeField] BaseTile ClosestWater;

	[SerializeField] BaseTile ClosestWorkbench;
	[SerializeField] Character Character;

	[SerializeField] BuildManager BuildManager;

	public Text Text;

	public int SelectedWeaponIndex = -1;
	public int LatestWeaponIndex = -2;
	public BaseTile LatestTile;

	private void Update()
	{
		CheckNearbyWater();
		CheckNearbyResources();
		CheckWorkbench();
		CheckWeaponOnHand();
	}

	public Resource GetCurrentWeapon()
		=> Character.BaseInventory.ResourcesInBag.ElementAtOrDefault(SelectedWeaponIndex)? .Resource ?? null;
	
	void CheckWeaponOnHand()
	{
		if (IsTool(SelectedWeaponIndex) && LatestWeaponIndex != SelectedWeaponIndex)
		{
			LatestWeaponIndex = SelectedWeaponIndex;
			var racketAttacker = GetComponentInChildren<RacketAttacker>();
			var sprite = racketAttacker.GetComponentInChildren<SpriteRenderer>();

			sprite.sprite = Character.BaseInventory.ResourcesInBag.ElementAt(SelectedWeaponIndex).Resource.Tile.sprite;
		}
	}

	void ClearOnHand()
	{
		LatestWeaponIndex = -2; 
		SelectedWeaponIndex = -1;
		var racketAttacker = GetComponentInChildren<RacketAttacker>();
		var sprite = racketAttacker.GetComponentInChildren<SpriteRenderer>();

		sprite.sprite = null;
	}

	void CheckNearbyWater()
	{
		if (Character.HasResourceNearby())
		{
			ClosestWater = Character.GetClosestWater(new List<BaseTile>());

			if(ClosestWater != null)
			{
				ClosestWater.enabled = true;
				ClosestWater.GetComponent<SpriteRenderer>().enabled = true;
				ClosestWater.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
			}
		} 
		else
		{
			ClosestWater = null;
		}

		if (ClosestWater && Input.GetKeyDown(KeyCode.E))
			Character.AddStamina(20);
	}

	void CheckNearbyResources()
	{
		if (ClosestResource != null)
		{
			ClosestResource.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
			ClosestResource.GetComponentsInChildren<Text>()[1].enabled = false;
		}

		if (Character.HasResourceNearby())
		{
			var closestResources = new List<BaseTile>();
			var tool = GetCurrentTool();

			if (tool == null) return;

			for (int i = 0; i < 9; i++)
			{
				ClosestResource = Character.GetClosestResource(closestResources);

				if (tool.Resource.CanGetResources && tool.Resource.GettableResources.Contains(ClosestResource.Resource))
				{
					ClosestResource.enabled = true;
					ClosestResource.GetComponent<SpriteRenderer>().enabled = true;
					ClosestResource.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
					break;
				}

				ClosestResource = null;
				closestResources.Add(ClosestResource);
			}
		}
		else
		{
			ClosestResource = null;
		}

		if (ClosestResource != null && Input.GetKeyDown(KeyCode.Space))
		{
			if(Character.ConsumeStamina(1))
				Character.BaseInventory.AddResource(ClosestResource.Resource, 1);
		}

	}
	
	void CheckWorkbench()
	{
		if (ClosestWorkbench != null)
			ClosestWorkbench.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);

		if (Character.HasWorkbenchNearby())
		{
			ClosestWorkbench = Character.GetClosestWorkbench();
			ClosestWorkbench.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
			BuildManager.OpenOn(ClosestWorkbench.transform.position + new Vector3(-0.5f,1,0));
		}
		else
		{
			BuildManager.Close();
			ClosestWorkbench = null;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) CreateResource(0);
		if (Input.GetKeyDown(KeyCode.Alpha2)) CreateResource(1);
		if (Input.GetKeyDown(KeyCode.Alpha3)) CreateResource(2);
		if (Input.GetKeyDown(KeyCode.Alpha4)) CreateResource(3);
		if (Input.GetKeyDown(KeyCode.Alpha5)) CreateResource(4);
		if (Input.GetKeyDown(KeyCode.Alpha6)) CreateResource(5);

	}

	void CreateResource(int index)
	{
		if (Input.GetKey(KeyCode.R))
		{
			Character.BaseInventory.DropResource(index);
			ClearOnHand();
		} 
		else if (ClosestWorkbench != null)
		{
			Debug.Log($"WORKING ON {index}");
			var resource = BuildManager.CreateItem(index, Character.BaseInventory.ResourcesInBag);
			if (resource != null)
			{
				Character.BaseInventory.AddResource(resource, 1);
				Debug.Log($"CREATED: {resource}");
			}
			else
			{
				Debug.Log($"NOT POSSIBLE");
			}
			return;
		}
		else if (IsTool(index))
		{
			SelectedWeaponIndex = index;
		}

		
	}

	ResourceInBag GetCurrentTool()
		=> IsTool(SelectedWeaponIndex) ? Character.BaseInventory.ResourcesInBag.ElementAt(SelectedWeaponIndex) : null;

	bool IsTool(int index)
	{
		if (index < 0 || !Character.BaseInventory.ResourcesInBag.Any() || Character.BaseInventory.ResourcesInBag.ElementAtOrDefault(index) == null) return false;
		
		var resourceAtIndex =  Character.BaseInventory.ResourcesInBag.ElementAt(index);
		return resourceAtIndex.Resource.CanHitEnemies || resourceAtIndex.Resource.CanGetResources;
	}
}
