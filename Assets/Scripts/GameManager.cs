using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public float TurnDelay = .1f;
    public static GameManager Instance = null;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;
    [HideInInspector]public bool PlayersTurn = true;

    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
        enemies = new List<Enemy>();
        this.BoardScript.GetComponent<BoardManager>();
        this.InitGame();
    }

    public void GameOver()
    {
        this.enabled = false;
    }

    private void InitGame()
    {
        this.enemies.Clear();
        this.BoardScript.SetupScene(level);
    }

    private IEnumerator MoveEnemies()
    {
        this.enemiesMoving = transform;

        yield return new WaitForSeconds(this.TurnDelay);
        if(this.enemies.Count == 0)
        {
            yield return new WaitForSeconds(this.TurnDelay);
        }

        for (int i = 0; i < this.enemies.Count; i++)
        {
            this.enemies[i].MoveEnemy();

            yield return new WaitForSeconds(this.enemies[i].MoveTime);
        }

        this.PlayersTurn = true;
        this.enemiesMoving = false;
    }

    public void Update()
    {
        if(this.PlayersTurn || this.enemiesMoving)
        {
            return;
        }

        StartCoroutine(MoveEnemies());
    }

    public void AddEnemyToList(Enemy script)
    {
        this.enemies.Add(script);
    }
}

