using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameInfo : MonoBehaviour
{
	public static GameInfo Instance;

	public float PlayerSpeed;
	public float PlayerReloadTime;
	public float PointsToWin;

	public float EnemySpeed;
	public float EnemyReloadTime;
	public float PointToKillEnemy;
	
	public EnemyBall[] EnemyBalls;
	public Ball[] Balls;
	public GameObject[] SpawnPoints;
	public GameObject[] HealthHearts;
	public GameObject[] Enemies;
	public GameObject[] Stars;
	
	public GameObject LosePanel;
	public GameObject WinPanel;
	public UnityEngine.UI.Text ScoreText;
	
	public float EasyEmemyMultiplier;
	public float MediumEmemyMultiplier;
	public float HardEnemyMultiplier;

	public ParticleSystem particles;

	public bool isPlaying = false;
	public float enemySizeByY;

	public void Awake()
	{
		if(Instance != this)
		{
			Instance = this;
		}
		isPlaying = true;
		Time.timeScale = 1;
		enemySizeByY = Enemies[0].GetComponent<CapsuleCollider2D>().bounds.size.y;
		StartCoroutine(SpawnEnemies());
	}

	IEnumerator SpawnEnemies()
	{
		SpawnEnemy(0);
		//Spawn 2 Enemy after 3s.
		yield return new WaitForSeconds(5);
		SpawnEnemy(1);
		//Spawn 3 Enemy after 4s;
		yield return new WaitForSeconds(8);
		SpawnEnemy(2);
	}

	private void SpawnEnemy(int whichEnemy)
	{
		Enemies[whichEnemy].SetActive(true);
		Enemies[whichEnemy].GetComponent<Enemy>().Respawn();
	}
	public void ShowScore(int hippoScore)
	{
		print(hippoScore);
		ScoreText.text = hippoScore.ToString();
	}

	public void ShowWinPanel()
	{
		particles.Play();
		isPlaying = false;
		int stars = Player.Instance.GetCurrentHealth();
		for(int i = 0; i < stars; i++)
		{
			Stars[i].SetActive(true);
		}
		WinPanel.SetActive(true);
	}

	private void Update()
	{
		if (!isPlaying) {
			foreach(GameObject enemy in Enemies)
			{
				enemy.GetComponent<Rigidbody2D>().velocity = new Vector2(5f,0);
			}
			Time.timeScale = 0;
		}
	}

	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void ShowHealth(int hippoHealth)
	{
		foreach(GameObject heart in HealthHearts)
		{
			if (hippoHealth <= 0)
			{
				heart.SetActive(false);
			}
			else
			{
				heart.SetActive(true);
				hippoHealth--;
			}
			
		}
	}

	public void ShowLosePanel()
	{
		isPlaying = false;
		LosePanel.SetActive(true);
	}

}
