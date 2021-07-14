using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBall : MonoBehaviour
{
	public bool isUsing = false;
	public Vector2 velocity;
	public float speed = 3;

	private void Start()
	{
	}

	private void OnBecameVisible()
	{
		GetComponent<Rigidbody2D>().velocity = velocity;
		isUsing = true;
	}

	private void OnBecameInvisible()
	{
		isUsing = false;
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer("Hippo"))
		{
			other.GetComponent<Player>().TakeDamage();
			transform.position = new Vector3(100f, 10f, 0);
		}
	}
}
