using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ClockManager: SingletonMonoBehaviour<ClockManager>
{
	Text ClockText;
	public int ClockTime = 0;
	public static UnityEvent OnChange;

	void Start()
	{
		ClockText = GetComponentInChildren<Text>();
		StartCoroutine(TimerRoutine());
	}

	IEnumerator TimerRoutine()
	{
		WaitForSeconds delay = new WaitForSeconds(1);
		while (true)
		{
			ClockTime += 1;
			yield return delay;
		}
	}
	
	void Update()
	{
		string second = LeadingZero(ClockTime % 60);
		string minute = LeadingZero(ClockTime / 60);
		ClockText.text = minute + ":" + second;
	}

	string LeadingZero(int n) => n.ToString().PadLeft(2, '0');
}