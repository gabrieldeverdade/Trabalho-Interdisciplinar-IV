using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
	public Animator Animator;

	public void Start()
	{
		Animator = GetComponentInChildren<Animator>();
	}

	public void Play(string animationName)
	{
		Animator.Play(animationName);
	}
}
