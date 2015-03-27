using UnityEngine;
using System.Collections;

public abstract class MovingObject : MonoBehaviour
{
    public float MoveTime = 0.1f;
    public LayerMask BlockLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    protected virtual void Start()
    {
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        this.rb2D = this.GetComponent<Rigidbody2D>();
        this.inverseMoveTime = 1f / this.MoveTime;
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = this.transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        this.boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, this.BlockLayer);
        this.boxCollider.enabled = true;

        if (hit.transform == null)
        {
            StartCoroutine(this.SmoothMovement(end));

            return true;
        }

        return false;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (this.transform.position - end).sqrMagnitude;

        while(sqrRemainingDistance > float.Epsilon)
        {
            var newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            this.rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
    }

    protected virtual void AttemptMove<T>(int xDir,int yDir) where T : Component
    {
        RaycastHit2D hit;
        bool canMove = this.Move(xDir, yDir, out hit);

        if(hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        if(!canMove  && hitComponent != null)
        {
            this.OnCanMove(hitComponent);
        }
    }

    protected abstract void OnCanMove<T>(T component) where T : Component;
}

