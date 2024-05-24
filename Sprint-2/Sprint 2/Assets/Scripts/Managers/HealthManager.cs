using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
	[SerializeField] Image healthBar;

	public void ChangeBar(float totalLife)
	{
		healthBar.fillAmount = totalLife;
	}
}