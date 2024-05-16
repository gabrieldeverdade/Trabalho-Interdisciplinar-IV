using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager: MonoBehaviour
{
	private static ClockManager _instance;
	public static ClockManager Instance { get { return _instance; } }

	public int ClockTime = 0;
	Text ClockText;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}

		ClockText = GetComponentInChildren<Text>();
	}

	void Start()
	{
		StartCoroutine(TimerRoutine());
	}

	IEnumerator TimerRoutine()
	{
		WaitForSeconds delay = new WaitForSeconds(1);
		while (true)
		{
			ClockTime += 1;
			OnClockChange();
			yield return delay;
		}
	}

	void OnClockChange()
	{
	}

	private void Update()
	{
		string second = LeadingZero(ClockTime % 60);
		string minute = LeadingZero(ClockTime / 60);
		ClockText.text = minute + ":" + second;
	}


	string LeadingZero(int n) => n.ToString().PadLeft(2, '0');
}