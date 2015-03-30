using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float LevelStartDelay = 2f;
    public float TurnDelay = .1f;
    public static GameManager Instance = null;
    public BoardManager BoardScript;
    public int PlayerFoodPoints = 100;
    [HideInInspector]public bool PlayersTurn = true;

    private Text levelText;
    private int level = 1;
    private GameObject levelImage;
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;

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
        this.levelText.text = "After " + this.level + " days, you starved.";
        this.levelImage.SetActive(true);
        this.enabled = false;
    }

    private void InitGame()
    {
        this.doingSetup = transform;
        this.levelImage = GameObject.Find("LevelImage");
        this.levelText = GameObject.Find("LevelText").GetComponent<Text>();
        this.levelText.text = "Day " + this.level;
        this.levelImage.SetActive(true);
        Invoke("HideLevelImage", this.LevelStartDelay);

        this.enemies.Clear();
        this.BoardScript.SetupScene(level);
    }

    private void HideLevelImage()
    {
        this.levelImage.SetActive(false);
        this.doingSetup = false;
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

    private void OnLevelWasLoaded(int index)
    {
        this.level++;
        this.InitGame();
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

