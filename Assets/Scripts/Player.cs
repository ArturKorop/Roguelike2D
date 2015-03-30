using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int WallDamage = 1;
    public int PointsPerFood = 10;
    public int PointsPerSoda = 20;
    public float RestartLevelDelay = 1f;
    public Text FoodText;

    public AudioClip MoveSound1;
    public AudioClip MoveSound2;
    public AudioClip EatSound1;
    public AudioClip EatSound2;
    public AudioClip DrinkSound1;
    public AudioClip DrinkSound2;
    public AudioClip GameOverSound;

    private Animator animator;
    private int food;

    protected override void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.food = GameManager.Instance.PlayerFoodPoints;
        this.FoodText.text = "Food: " + this.food;

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
        this.FoodText.text = "-" + loss + " Food: " + this.food;
        this.ChickIfGameOver();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        this.food--;
        this.FoodText.text = "Food: " + this.food;

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        if(this.Move(xDir, yDir, out hit))
        {
            SoundManager.Instance.RandomixzeSfx(this.MoveSound1, this.MoveSound2);
        }

        this.ChickIfGameOver();

        GameManager.Instance.PlayersTurn = false;
    }

    protected override void OnCanMove<T>(T component)
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
            SoundManager.Instance.PlaySingle(this.GameOverSound);
            SoundManager.Instance.MusicSource.Stop();
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
            this.FoodText.text = "+" + this.PointsPerFood + " Food: " + this.food;
            SoundManager.Instance.RandomixzeSfx(this.EatSound1, this.EatSound2);
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Soda")
        {
            this.food += this.PointsPerSoda;
            this.FoodText.text = "+" + this.PointsPerSoda + " Food: " + this.food;
            SoundManager.Instance.RandomixzeSfx(this.DrinkSound1, this.DrinkSound2);
            other.gameObject.SetActive(false);
        }
    }
}
