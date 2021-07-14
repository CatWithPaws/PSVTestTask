using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	[SerializeField] private Vector2 BallVelocity = new Vector2(24, 12);
	[SerializeField] private Vector2 startVelocity = new Vector2(24,12);
	[SerializeField] private float gravity = 24;
	public Vector2 shootPos;
	public float throwMultiplier;
	public bool isUsing = false;
	

	private void FixedUpdate()
	{
		if (!isUsing) return;
		if (transform.position.y < shootPos.y - Player.Instance.GetComponent<CapsuleCollider2D>().bounds.size.y) HideBall();
		BallVelocity.y -= gravity * Time.fixedDeltaTime;
		GetComponent<Rigidbody2D>().velocity = BallVelocity;
	}

	private void OnBecameVisible()
	{
		BallVelocity *= throwMultiplier;
		isUsing = true;
		
	}

	private void OnBecameInvisible()
	{
		isUsing = false;
		BallVelocity = startVelocity;
		
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.layer == LayerMask.NameToLayer("Enemies"))
		{
			other.GetComponent<Enemy>().TakeDamage();
			HideBall();
		}
	}

	private void HideBall()
	{
		transform.position = new Vector3(-100f, 10f, 0);
	}

}
