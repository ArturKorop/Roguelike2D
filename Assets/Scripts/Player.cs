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
    private Vector2 touchOrigin = -Vector2.one;

    protected override void Start()
    {
        this.animator = this.GetComponent<Animator>();
        this.food = GameManager.Instance.PlayerFoodPoints;
        this.FoodText.text = "Food: " + this.food;

        base.Start();
    }

    public void Update()
    {
        if (!GameManager.Instance.PlayersTurn)
        {
            return;
        }

        int horizontal = 0;
        int vertical = 0;

#if UNITY_STANDALONE || UNITY_WEBPLAYER// || UNITY_EDITOR

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE || UNITY_WP_8_1
//Check if Input has registered more than zero touches
			if (Input.touchCount > 0)
			{
				//Store the first touch detected.
				Touch myTouch = Input.touches[0];
				
				//Check if the phase of that touch equals Began
				if (myTouch.phase == TouchPhase.Began)
				{
					//If so, set touchOrigin to the position of that touch
					touchOrigin = myTouch.position;
				}
				
				//If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
				{
					//Set touchEnd to equal the position of this touch
					Vector2 touchEnd = myTouch.position;
					
					//Calculate the difference between the beginning and end of the touch on the x axis.
					float x = touchEnd.x - touchOrigin.x;
					
					//Calculate the difference between the beginning and end of the touch on the y axis.
					float y = touchEnd.y - touchOrigin.y;
					
					//Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
					touchOrigin.x = -1;
					
					//Check if the difference along the x axis is greater than the difference along the y axis.
					if (Mathf.Abs(x) > Mathf.Abs(y))
						//If x is greater than zero, set horizontal to 1, otherwise set it to -1
						horizontal = x > 0 ? 1 : -1;
					else
						//If y is greater than zero, set horizontal to 1, otherwise set it to -1
						vertical = y > 0 ? 1 : -1;
				}
			}
			
#endif
        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            this.AttemptMove<Wall>(horizontal, vertical);
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
