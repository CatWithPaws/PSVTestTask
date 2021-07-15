using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	int pointsTakenAfterHitting;

	Vector3 target;
	bool canShoot;

	[SerializeField] Transform shootPos;
	private delegate void VoidFunc();
	VoidFunc[] InitEnemy = new VoidFunc[3];
	[SerializeField] int ballNumber;
	//ONLY 0 or 1!!!
	[SerializeField] int spawnPoint;
	[SerializeField] ParticleSystem particles;
	private float ballSpeedMultiplier;

	

	string[] AnimalName = {"Cat", "Dog", "Fox", "Hippo", "Leo", "Pig", "Puma", "Raccoon" };
	string[] AnimalStatusInFamily = { "Grandpa", "Grandma", "Mama", "Papa"};
	private void Start()
	{
		InitEnemy[0] = InitEasyEnemy;
		InitEnemy[1] = InitMediumEnemy;
		InitEnemy[2] = InitHardEnemy;

		InitEntity();

	}

	private void OnBecameInvisible()
	{
		canShoot = false;
	}

	private void OnBecameVisible()
	{
		canShoot = true;
	}

	void SetRandomSkin()
	{
		int rndNum = Random.Range(0, AnimalName.Length);
		string _randomName = AnimalName[rndNum];
		rndNum = Random.Range(0, AnimalStatusInFamily.Length);
		string _randomStatus = AnimalStatusInFamily[rndNum];

		entitySkeletonAnimation.skeleton.SetSkin(_randomName + _randomStatus);
	}

	public void Respawn()
	{
		SetRandomSkin();
		gameObject.SetActive(true);
		transform.position = GameInfo.Instance.SpawnPoints[spawnPoint].transform.position;
		base.InitEntity();
		StartCoroutine(GoToField());
	}

	IEnumerator GoToField()
	{
		if (transform.position.y > 0)
		{
			entityRB.velocity = -Vector3.up * speed;
		}
		else if (transform.position.y < 0)
		{
			entityRB.velocity = Vector3.up * speed;
		}
		yield return new WaitForSeconds(1f);
		InitEntity();
	}

	protected override void InitEntity()
	{
		StopAllCoroutines();
		startHealth = 1;
		entityRB.velocity = Vector3.up * speed;
		int rnd = Random.Range(0, 2);
		speed = GameInfo.Instance.EnemySpeed;
		InitEnemy[rnd]();
		StartCoroutine(RandomMove());
		StartCoroutine(TimedShoot());
	}
	protected void MoveToPosition()
	{
		Vector3 newPosition = Vector3.zero;
		newPosition = Vector3.MoveTowards(transform.position, target, 0.02f);
		entityRB.MovePosition(newPosition);
	}

	private void InitEasyEnemy()
	{
		speed *= GameInfo.Instance.EasyEmemyMultiplier;
		ballSpeedMultiplier = GameInfo.Instance.EasyEmemyMultiplier;
		pointsTakenAfterHitting = 1;
	}
	private void InitMediumEnemy()
	{
		speed *= GameInfo.Instance.MediumEmemyMultiplier;
		ballSpeedMultiplier = GameInfo.Instance.MediumEmemyMultiplier;
		pointsTakenAfterHitting = 2;
	}
	private void InitHardEnemy()
	{
		speed *= GameInfo.Instance.HardEnemyMultiplier;
		ballSpeedMultiplier = GameInfo.Instance.HardEnemyMultiplier;
		pointsTakenAfterHitting = 3;
	}



	protected void Kill()
	{
		Player.Instance.AddPoints(pointsTakenAfterHitting);
		StartCoroutine(RespawnAfterSeconds(3));
		entityRB.velocity = new Vector2(10, 0);
	}

	private IEnumerator RespawnAfterSeconds(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		isAlive = true;
		Respawn();
	}

	void SetNewTargetPosition()
	{
		target = new Vector3(transform.position.x, Random.Range(minYPosition, maxYPosition), transform.position.z);
	}

	void Shoot()
	{
		EnemyBall enemyBall = GameInfo.Instance.GetEnemyBall();
		
		
		Vector3 _hippoDirection =  Player.Instance.gameObject.transform.position - transform.position ;
		_hippoDirection = _hippoDirection.normalized;
		_hippoDirection.z = 0;
		enemyBall.velocity = _hippoDirection * enemyBall.speed * ballSpeedMultiplier;
		enemyBall.transform.position = shootPos.position;
	}

	private void Update()
	{
		SetAnimationByVelocity();
		if (!GameInfo.Instance.isPlaying) StopAllCoroutines();
		if (transform.position.y > maxYPosition)
		{
			entityRB.velocity = -Vector3.up * speed;
		}
		else if (transform.position.y < minYPosition)
		{
			entityRB.velocity = Vector3.up * speed;
		}
	}

	IEnumerator TimedShoot()
	{
		while (isAlive && GameInfo.Instance.isPlaying)
		{
			yield return new WaitForSeconds(5f);
			if(canShoot) Shoot();
		}
	}


	IEnumerator RandomMove()
	{
		while (isAlive && GameInfo.Instance.isPlaying) {
			Vector3 _velocityAfterChanging;
			float num = Random.Range(0f, 1f);
			if (num > 0.7f) entityRB.velocity = -entityRB.velocity;
			_velocityAfterChanging = entityRB.velocity;
			if(num < 0.3f)
			{
				entityRB.velocity = Vector3.zero;
				yield return new WaitForSeconds(1.5f);
				entityRB.velocity = _velocityAfterChanging;
			}
			yield return new WaitForSeconds(1f);
			}
		}
	public override void TakeDamage()
	{
		isAlive = false;
		print("dead");
		particles.Play();
		if(--currentHealth <= 0)
		{
			Kill();
		}
	}

	}



