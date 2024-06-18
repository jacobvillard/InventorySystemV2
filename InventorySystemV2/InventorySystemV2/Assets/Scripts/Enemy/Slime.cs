using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Slime : Enemy
{
    
    public int health = 5;
    public int damage = 1;
    private Animator _anim;
    bool touchingPlayer = false;
    bool canAttack = true;
    bool dying = false;
    private Collision2D playerCollider;



    void Start()
    {
        _anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
        if (touchingPlayer && canAttack && !dying) StartCoroutine(AttackCoroutine());
    }


    public override void Movement()
    {
        transform.Translate(Vector3.left * 0.5f * Time.deltaTime);
    }

    public override void Damage(int damageTaken)
    {
        health -= damageTaken;
        _anim.SetTrigger("Hurt");
        if (health <= 0 && !dying) Die();
        
    }

    public override void Die()
    {
        dying = true;
        _anim.SetTrigger("Die");
        Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
        var rb2d = this.gameObject.GetComponent<Rigidbody2D>();
        rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
        Destroy(gameObject, 1f);
    }



    IEnumerator AttackCoroutine()
    {
        canAttack = false;
        if(playerCollider != null)playerCollider.gameObject.GetComponent<Controller>().Damage(damage);
        yield return new WaitForSeconds(1f);
        canAttack = true;

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            playerCollider = col;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
}
