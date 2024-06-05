using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClockManager : SingletonMonoBehaviour<ClockManager>
{
	[SerializeField] Text ClockText;
	[SerializeField] Camera Camera;

	public int ClockTime { get; private set; } = 0;
	public float TimeDelta { get; private set; } = 0;

	void Start() => StartCoroutine(TimerRoutine());

	void Update()
	{
		string second = LeadingZero(ClockTime % 60);
		string minute = LeadingZero(ClockTime / 60);
		ClockText.text = minute + ":" + second;
		TimeDelta += Time.deltaTime;
	}

	string LeadingZero(int n) => n.ToString().PadLeft(2, '0');

	IEnumerator TimerRoutine()
	{
		WaitForSeconds delay = new WaitForSeconds(1);
		while (true)
		{
			ClockTime += 1;
			yield return delay;
		}
	}
}