using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
	[SerializeField] Image healthBar;
	public float CurrentTotal;

	public void ChangeBar(float totalLife)
	{
		CurrentTotal = totalLife;
		healthBar.fillAmount = totalLife;
	}
}