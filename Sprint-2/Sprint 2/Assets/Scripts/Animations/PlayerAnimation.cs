using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	public Animator Animator;
	public AudioSource AudioSource;
	public Direction CurrentDirection;

	public void Start()
	{
		Animator = GetComponentInChildren<Animator>();
		AudioSource = GetComponentInChildren<AudioSource>();
	}

	public void Play(string animationName)
	{
		if (Animator == null) return; 

		Animator.Play(animationName);
	}

	public void Animate(Vector2Int direction, double speed)
	{
		if (Animator == null) return;

		var latestDirection = DirectionManager.GetDirectionDegrees(direction);

		if ((direction.magnitude / speed) > 0.001)
			CurrentDirection = latestDirection;

		Play(
			direction.magnitude < 0.001
			? DirectionManager.GetIdleAnimation(CurrentDirection)
			: DirectionManager.GetWalkAnimation(CurrentDirection)
		);

		if(direction.magnitude >= 0.001)
		{
			Play(DirectionManager.GetWalkAnimation(CurrentDirection));
			AudioSource.time = 0;
			AudioSource.Play();
		}
		else
			Play(DirectionManager.GetIdleAnimation(CurrentDirection));

	}
}
