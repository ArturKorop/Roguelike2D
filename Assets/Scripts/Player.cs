using UnityEngine;
using System.Collections;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.food = GameManager.Instance.PlayerFoodPoints;

        base.Start();
    }

    public void Update()
    {
        if(!GameManager.Instance.PlayersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vercital = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vercital = (int)Input.GetAxisRaw("Vertical");

        if(horizontal != 0)
        {
            vercital = 0;
        }

        if(horizontal != 0 || vercital != 0)
        {
            this.AttemptMove<Wall>(horizontal, vercital);
        }
    }

    public void LoseFood(int loss)
    {
        this.animator.SetTrigger("PlayerHit");
        this.food -= loss;
        this.ChickIfGameOver();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        this.food--;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        this.ChickIfGameOver();

        GameManager.Instance.PlayersTurn = false;
    }

    protected override void OnCantMove<T>(T component)
    {
        var hitWall = component as Wall;
        hitWall.DamageWall(this.WallDamage);
        this.animator.SetTrigger("PlayerChop");
    }

    private void Restart()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

    private void OnDisable()
    {
        GameManager.Instance.PlayerFoodPoints = food;
    }

    private void ChickIfGameOver()
    {
        if(this.food <= 0)
        {
            GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Exit")
        {
            Invoke("Restart", this.RestartLevelDelay);
            this.enabled = false;
        }
        else if(other.tag == "Food")
        {
            this.food += this.PointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Soda")
        {
            this.food += this.PointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
