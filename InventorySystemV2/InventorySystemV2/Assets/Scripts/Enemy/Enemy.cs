using Interfaces;
using UnityEngine;

namespace Enemy {
    /// <summary>
    /// This is the base class for all enemies in the game.
    /// </summary>
    public abstract class Enemy : MonoBehaviour, IDamageable
    {

        public abstract void Damage(int damageTaken);

        public abstract void Movement();

        public abstract void Die();


    }
}
