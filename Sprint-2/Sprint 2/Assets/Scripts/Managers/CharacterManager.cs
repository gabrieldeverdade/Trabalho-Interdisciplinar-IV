using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
	[SerializeField] BaseTile ClosestResource;
	[SerializeField] BaseTile ClosestWorkbench;
	[SerializeField] Character Character;

	[SerializeField] BuildManager BuildManager;

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

		if (ClosestWorkbench != null && Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("WORKING");
			var resource = BuildManager.CreateItem(0, Character.BaseInventory.ResourcesInBag);
			if(resource != null)
			{
				Character.BaseInventory.AddResource(resource, 1);
				Debug.Log($"CREATED: {resource}");
			} else {
				Debug.Log($"NOT POSSIBLE");
			}
		}
	}
}
