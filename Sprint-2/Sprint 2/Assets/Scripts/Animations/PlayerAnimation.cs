using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	public Animator Animator;
	public Direction CurrentDirection;

	public void Start()
	{
		Animator = GetComponentInChildren<Animator>();
	}

	public void Play(string animationName)
	{
		Animator.Play(animationName);
	}

	public void Animate(Vector2Int direction, double speed)
	{
		var latestDirection = DirectionManager.GetDirectionDegrees(direction);

		if ((direction.magnitude / speed) > 0.001)
			CurrentDirection = latestDirection;

		Play(
			direction.magnitude < 0.001
			? DirectionManager.GetIdleAnimation(CurrentDirection)
			: DirectionManager.GetWalkAnimation(CurrentDirection)
		);
	}
}
