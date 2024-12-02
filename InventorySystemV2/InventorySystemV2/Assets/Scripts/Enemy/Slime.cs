using System.Collections;
using Player;
using UnityEngine;

namespace Enemy {
    /// <summary>
    /// The Slime class is a subclass of the Enemy class. It represents the Slime enemy in the game.
    /// </summary>
    public class Slime : Enemy {
        
        // The animator component of the slime.
        private static readonly int Hurt = Animator.StringToHash("Hurt");
        private static readonly int Die1 = Animator.StringToHash("Die");
        /// <summary>
        /// The health of the slime.
        /// </summary>
        public int health = 5;
        /// <summary>
        /// The damage the slime deals to the player.
        /// </summary>
        public int damage = 1;
        
        // Private fields
        private Animator anim;
        private bool canAttack = true;
        private bool dying;
        private Collision2D playerCollider;
        private bool touchingPlayer;

        // Start is called before the first frame update
        private void Start() {
            anim = gameObject.GetComponent<Animator>();
        }

        // Update is called once per frame
        private void Update() {
            Movement();
            if (touchingPlayer && canAttack && !dying) StartCoroutine(AttackCoroutine());
        }

        /// <summary>
        /// Detects when the slime collides with the player.
        /// </summary>
        /// <param name="col"></param>
        private void OnCollisionEnter2D(Collision2D col) {
            if (!col.gameObject.CompareTag("Player")) return;
            touchingPlayer = true;
            playerCollider = col;
        }

        /// <summary>
        /// Detects when the slime stops colliding with the player.
        /// </summary>
        /// <param name="col"></param>
        private void OnCollisionExit2D(Collision2D col) {
            if (col.gameObject.CompareTag("Player")) touchingPlayer = false;
        }
        
        /// <summary>
        /// Overrides the Movement method from the Enemy class. It makes the slime move to the left.
        /// </summary>
        public override void Movement() {
            transform.Translate(Vector3.left * 0.5f * Time.deltaTime);
        }

        /// <summary>
        /// Damages the slime by a certain amount. If the slime's health reaches 0, it dies.
        /// </summary>
        /// <param name="damageTaken"></param>
        public override void Damage(int damageTaken) {
            health -= damageTaken;
            anim.SetTrigger(Hurt);
            if (health <= 0 && !dying) Die();
        }
        
        /// <summary>
        /// Destroys the slime game object.
        /// </summary>
        public override void Die() {
            dying = true;
            anim.SetTrigger(Die1);
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            var rb2d = gameObject.GetComponent<Rigidbody2D>();
            rb2d.constraints = RigidbodyConstraints2D.FreezePosition;
            Destroy(gameObject, 1f);
        }

        
        /// <summary>
        /// Attacks the player by dealing damage to it.
        /// </summary>
        /// <returns></returns>
        private IEnumerator AttackCoroutine() {
            canAttack = false;
            playerCollider?.gameObject.GetComponent<Controller>().Damage(damage);
            yield return new WaitForSeconds(1f);
            canAttack = true;
        }
    }
}