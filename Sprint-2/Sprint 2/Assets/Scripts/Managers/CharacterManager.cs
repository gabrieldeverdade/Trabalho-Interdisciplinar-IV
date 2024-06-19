using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
	[SerializeField] BaseTile ClosestResource;
	[SerializeField] BaseTile ClosestWorkbench;
	[SerializeField] Character Character;

	[SerializeField] BuildManager BuildManager;

	public int SelectedWeaponIndex = 0;

	private void Update()
	{
		CheckNearbyResources();
		CheckWorkbench();
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
			ClosestResource = Character.GetClosestResource();
			ClosestResource.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
			ClosestResource.GetComponentsInChildren<Text>()[1].enabled = true;
		}
		else
			ClosestResource = null;

		if (ClosestResource != null && Input.GetKeyDown(KeyCode.E))
		{
			Debug.Log("CONSUMING");
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
		if (ClosestWorkbench != null)
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
		}
		else 
		{
			SelectedWeaponIndex = index;
		}
	}
}