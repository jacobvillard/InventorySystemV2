using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{

    public abstract void Damage(int damageTaken);

    public abstract void Movement();

    public abstract void Die();


}
