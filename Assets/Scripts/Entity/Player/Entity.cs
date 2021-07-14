using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SkeletonAnimation))]
public abstract class Entity : MonoBehaviour
{

		protected float maxYPosition = 4.67f;
	protected float minYPosition = -6.32f;

	[SerializeField] protected float speed = 1;

	[SerializeField] protected int startHealth;
	[SerializeField] protected int currentHealth;

	[SerializeField] protected SkeletonAnimation entitySkeletonAnimation;

	[SerializeField] protected Rigidbody2D entityRB;

	[SerializeField] protected bool isAlive = true;



	private void Awake()
	{
	}

	protected virtual void InitEntity()
	{
		entityRB = GetComponent<Rigidbody2D>();
		entitySkeletonAnimation = GetComponent < SkeletonAnimation>();
		currentHealth = startHealth;
	}

	protected void SetAnimationByVelocity()
	{
		if (entityRB.velocity.y != 0)
		{
			entitySkeletonAnimation.AnimationName = "run";
		}
		if(entityRB.velocity.y == 0)
		{
			entitySkeletonAnimation.AnimationName = "Idle";
		}
	}

	public abstract void TakeDamage();

}
