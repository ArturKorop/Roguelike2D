using UnityEngine;
using System.Collections;

public class Enemy : MovingObject
{
    public int PlayerDamage;

    private Animator animator;
    private Transform target;
    private bool SkipMove;

    protected override void Start()
    {
        GameManager.Instance.AddEnemyToList(this);
        this.animator = this.GetComponent<Animator>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        if(this.SkipMove)
        {
            this.SkipMove = false;

            return;
        }
        
        base.AttemptMove<T>(xDir, yDir);

        this.SkipMove = true;
    }

    public void MoveEnemy()
    {
        var xDir = 0;
        var yDir = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
        {
            yDir = target.position.y > transform.position.y ? 1 : -1;
        }
        else
        {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        this.AttemptMove<Player>(xDir, yDir);
    }

    protected override void OnCanMove<T>(T component)
    {
        var hitPlayer = component as Player;

        this.animator.SetTrigger("EnemyAttack");

        hitPlayer.LoseFood(this.PlayerDamage);
    }
}