using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	N = 0,
	NW = 1,
	W = 2,
	SW	= 3,
	S = 4,
	SE = 5,
	E = 6,
	NE = 7
}


public class PlayerAnimation : MonoBehaviour
{
	private Animator animator;

	// Start is called before the first frame update
	void Awake()
	{
		animator = GetComponent<Animator>();
	}

	public void SetDirection(Vector2 direction)
	{
		var directionArray = direction.magnitude < 0.01 ? "Static" : "Run";
		animator.Play($"{directionArray} SE");
	}

	string GetDirection(Vector2 direction)
	{
		float step = 360 / 8;
		float offset = step / 2;
		float angle = Vector2.SignedAngle(Vector2.up, direction.normalized);

		angle += offset;
		if (angle < 0) angle += 360;

		float stepCount = angle / step;

		return Enum.GetName(typeof(Direction), Mathf.FloorToInt(stepCount));
	}

}
