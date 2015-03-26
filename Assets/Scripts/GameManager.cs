using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public BoardManager BoardScript;

    private int level = 3;

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

        this.BoardScript.GetComponent<BoardManager>();
        this.InitGame();
    }

    public void Update()
    {

    }

    private void InitGame()
    {
        this.BoardScript.SetupScene(level);
    }
}
