using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Sprite dmgSpirte;
    public int Hp = 4;

    public AudioClip ChopSound1;
    public AudioClip ChopSound2;

    private SpriteRenderer spriteRenderer;

    public void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void DamageWall(int loss)
    {
        SoundManager.Instance.RandomixzeSfx(this.ChopSound1, this.ChopSound2);
        this.spriteRenderer.sprite = this.dmgSpirte;
        this.Hp -= loss;
        if (this.Hp <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}
