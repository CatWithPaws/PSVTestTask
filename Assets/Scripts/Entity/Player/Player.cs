using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
	public static Player Instance;
	[SerializeField] int currentPoints;
	[SerializeField] private int startPoints = 0;
	[SerializeField] int needPoints;
	[SerializeField] Transform ShootPosition;
	[SerializeField] Ball[] balls;


	[SerializeField] float reloadTime;
	[SerializeField] bool canShoot = true;
	public float ThrowMultiplier;

	public int GetCurrentHealth() => currentHealth;

	private void Awake()
	{
		if (Instance != this)
		{
			Instance = this;
		}
		currentHealth = 3;
	}
	private void Start()
	{
		
		GameInfo.Instance.ShowHealth(currentHealth);
		InitEntity();
	}
	protected override void InitEntity()
	{
		base.InitEntity();
		startHealth = 3;
		speed = GameInfo.Instance.PlayerSpeed;
		reloadTime = GameInfo.Instance.PlayerReloadTime;
		currentPoints = startPoints;
		currentHealth = startHealth;
	}

	public void Update()
	{

		SetAnimationByVelocity();
		ClampHippoMove();
	}

	private void ClampHippoMove()
	{
		transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, maxYPosition));
	}

	public void Shoot()
	{
		if (!canShoot) return;
		if (!GameInfo.Instance.isPlaying) return;
		foreach (Ball ball in balls)
		{
			if (!ball.isUsing && canShoot)
			{
				canShoot = false;
				if (ThrowMultiplier < 0.3f) ThrowMultiplier = 0.3f;
				ball.gameObject.transform.position = ShootPosition.position;
				ball.throwMultiplier = ThrowMultiplier;
				break;
			}
		}
	}

	public void SetCanShoot()
	{
		canShoot = true;
	}

	public  void SetYVelocity(float yDirection)
	{
		if(GameInfo.Instance.isPlaying)	entityRB.velocity = new Vector3(0, yDirection * speed, 0);
	}
	public void ResetVelocity()
	{
		if (GameInfo.Instance.isPlaying)
			entityRB.velocity = Vector3.zero;
	}
	public void AddPoints(int value)
	{
		currentPoints += value;
		GameInfo.Instance.ShowScore(currentPoints);
		if(currentPoints >= needPoints)
		{
			GameInfo.Instance.ShowWinPanel();
		}
	}

	public override void TakeDamage()
	{
		currentHealth--;
		GameInfo.Instance.ShowHealth(currentHealth);
		if (currentHealth <= 0) Die();
	}

	private void Die()
	{
		isAlive = false;
		GameInfo.Instance.ShowLosePanel();
	}

	public bool CanShoot()
	{
		return canShoot;
	}
	
}
