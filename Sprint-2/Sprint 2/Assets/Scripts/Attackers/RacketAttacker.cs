using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketAttacker : MonoBehaviour
{
	PlayerAnimation PlayerAnimation;
	Direction LatestDirection;

	Vector3 LatestPosition;
	Vector3 LatestRotation;

	int CurrentRotation = 1;
	int RotationDirection = 1;
	int RotatingSpeed = 10;

	public bool IsRotating => IsRotatingTowards || IsRotatingBackwards;
	bool IsRotatingTowards = false;
	bool IsRotatingBackwards = false;

	// Start is called before the first frame update
	void Start()
	{
		PlayerAnimation = GetComponentInParent<PlayerAnimation>();
		LatestDirection = PlayerAnimation.CurrentDirection;
		GetPosition();
		GetRotation();
		transform.position = transform.position + LatestPosition;
		SetRotation(LatestRotation);
	}

	void Update()
	{
		if (GetComponentInParent<CharacterManager>().GetCurrentWeapon() == null) return;

		if(LatestDirection != PlayerAnimation.CurrentDirection && !IsRotating)
		{
			LatestDirection = PlayerAnimation.CurrentDirection;
			transform.position = transform.position - LatestPosition;
			
			GetPosition();
			GetRotation();
			transform.position = transform.position + LatestPosition;
			SetRotation(LatestRotation);
		}

		if (Input.GetKeyDown(KeyCode.Space) && !IsRotating)
		{
			IsRotatingTowards = true;
			GetComponent<AudioSource>().time = 0;
			GetComponent<AudioSource>().Play();
			RotationDirection = GetStrikeRotationAmount() < 0 ? -1 : 1;
			CurrentRotation = 0;
		}

		if(IsRotating)
		{
			CurrentRotation += RotatingSpeed * RotationDirection;

			if(Mathf.Abs(CurrentRotation) > Mathf.Abs(GetStrikeRotationAmount()))
				IsRotatingTowards = false;

			SetRotation(SumWithDegrees(LatestRotation, new Vector3(0,0,CurrentRotation)));
		}
	}

	void GetPosition()
	{
		LatestPosition = PlayerAnimation.CurrentDirection switch
		{
			Direction.S => new Vector3(0.175f, 0.1f),
			Direction.SW => new Vector3(0.13f, 0.049f),
			Direction.SE => new Vector3(-0.09f, 0.049f),
			Direction.W => new Vector3(0.03f, 0.07f),
			Direction.E => new Vector3(-0.03f, 0.07f),
			Direction.N => new Vector3(-0.1f, 0.166f),
			Direction.NE => new Vector3(-0.1f, 0.166f),
			Direction.NW => new Vector3(-0.1f, 0.166f),
			_ => new Vector3(0, 0)
		};
	}

	void GetRotation()
	{
		LatestRotation = PlayerAnimation.CurrentDirection switch
		{
			Direction.N => new Vector3(0, 0, 30),
			Direction.W => new Vector3(0, 0, 50),
			Direction.S => new Vector3(0, 0, 180),
			Direction.E => new Vector3(0, 0, 320),
			_ => new Vector3(0, 0)
		};
	}

	int GetStrikeRotationAmount()
	{
		return PlayerAnimation.CurrentDirection switch
		{
			Direction.N => -90,
			Direction.NE => -90,
			Direction.NW => 90,
			Direction.W => 90,
			Direction.SW => 90,
			Direction.S => -90,
			Direction.SE => -90,
			Direction.E => -90,
			_ => 0
		};
	}

	void SetRotation(Vector3 newVector)
	{
		transform.eulerAngles = new Vector3(
				newVector.x,
				newVector.y,
				newVector.z
		);
	}
	
	Vector3 SumWithDegrees(Vector3 vect1, Vector3 vect2)
	{
		return new Vector3(
			(vect1.x + vect2.x) % 360,
			(vect1.y + vect2.y) % 360,
			(vect1.z + vect2.z) % 360
		);
	}
}
