using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	KeyboardController KeyboardController;

	void Start()
	{
		KeyboardController = GetComponentInParent<KeyboardController>();

	}

	void Update()
	{
	}
}
