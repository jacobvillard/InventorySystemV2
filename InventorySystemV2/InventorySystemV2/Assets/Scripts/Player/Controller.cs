using System.Collections;
using Generics;
using Interfaces;
using Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Player {
    /// <summary>
    ///     Player controller
    /// </summary>
    public class Controller : Singleton<Controller>, IDamageable {
        private void Start() {
            sprite = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update() {
            //Player health
            healthbarHUD.GetComponent<Bar>().UpdateBar(health, MaxHealth);
            health += regen * Time.deltaTime;

            //Player controls
            transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
            FlipSprite();

            if (Input.GetKeyDown(KeyCode.Space)) Jump(); //Jumping
            if (Input.GetMouseButtonDown(0)) //Attacking
            {
                if (item != null && item.itemdata.type == "weapon") {
                    var dmg = item.itemdata.desc.Split(' '); //The dmg is taken directly from the descritpion
                    Attack(int.Parse(dmg[0]));
                }
                else {
                    Attack(1);
                }
            }

            if (Input.GetMouseButtonDown(1) && !inventory.activeSelf) RightClick(); //Consuming

            if (Input.GetKeyDown(KeyCode.F1)) hotbar = 1; //Hotbar keybinds
            else if (Input.GetKeyDown(KeyCode.F2)) hotbar = 2;
            else if (Input.GetKeyDown(KeyCode.F3)) hotbar = 3;
            else if (Input.GetKeyDown(KeyCode.F4)) hotbar = 4;
            else if (Input.GetKeyDown(KeyCode.F5)) hotbar = 5;
            else if (Input.GetKeyDown(KeyCode.F6)) hotbar = 6;


            //Overlap sphere for dealing damage to ene,ies
            sphereRadius = item == null ? 1 : item.itemdata.width;
        }

        /// <summary>
        ///     Draws a sphere around the player to show the area of effect of the attack
        /// </summary>
        private void OnDrawGizmos() {
            Gizmos.DrawSphere(gameObject.transform.GetChild(0).position, sphereRadius);
        }

        #region Take Damage

        /// <summary>
        ///     Deals damage to the player
        /// </summary>
        /// <param name="dmg"></param>
        public void Damage(int dmg) {//Take damage to player 
            health -= dmg * resistanceMultiplier;

            if (!(health <= 0)) return;
            var currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        #endregion

        #region Flip Sprite

        /// <summary>
        /// Flips the sprite based on the player's direction
        /// </summary>
        private void FlipSprite() {
            if (Input.GetAxis("Horizontal") > 0f)
                sprite.flipX = false;
            else if (Input.GetAxis("Horizontal") < 0f) sprite.flipX = true;
        }

        #endregion

        /// <summary>
        /// Deals damage to all enemies in the area of effect
        /// </summary>
        /// <param name="attackDmg"></param>
        private void Attack(int attackDmg) {
            var attackAnim = gameObject.transform.GetChild(0).GetComponent<Animation>();
            if (!canJump) attackAnim.Play("WeaponSlashCircle");
            else if (!sprite.flipX) attackAnim.Play("WeaponSlash");
            else attackAnim.Play("WeaponSlashBackward");

            objectsInsideArea = Physics2D.OverlapCircleAll(gameObject.transform.GetChild(0).position, sphereRadius);
            foreach (var IsEnemy in objectsInsideArea)
                if (IsEnemy.gameObject.CompareTag("Enemy")) {
                    IsEnemy.GetComponent<IDamageable>().Damage(attackDmg * strength);
                    Debug.Log("d");
                }
        }


        /// <summary>
        /// Consumes the item in the player's inventory
        /// </summary>
        private void RightClick() {
            if (item == null) return;
            Interaction(item);
            eatAnim();
            removeItem = true;
            var weaponSprite = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            weaponSprite.sprite = null;
            item = null;
        }

        #region player variables

        [Header("Player Stats")] [SerializeField]
        private float speed = 1f;

        [SerializeField] private float jump = 5f;
        [SerializeField] private float health = 100f;
        [SerializeField] private int strength = 1;

        //Potion effects
        private float regen, resistanceMultiplier = 1f;
        private const float MaxHealth = 100f;

        //Item References
        public Item item;
        public bool removeItem;
        public int hotbar = 1;
        public int sphereRadius = 1;

        //Player variable(s)
        private bool canJump = true;

        #endregion

        #region player components

        [Header("Heatlh Bar")] [SerializeField]
        private GameObject healthbarHUD;


        [Header("Inventory Gameobject")] [SerializeField]
        private GameObject inventory;


        //Player components
        private SpriteRenderer sprite;
        private Animator anim;
        private Rigidbody2D rb;

        private Collider2D[] objectsInsideArea;

        #endregion

        #region Jump

        /// <summary>
        /// Allows the player to jump
        /// </summary>
        private void Jump() {
            if (!canJump) return;
            canJump = false;
            anim.SetBool("Walk", true);
            rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
            Invoke("ResetJump", jump / 5);
        }

        /// <summary>
        /// Resets the player's jump
        /// </summary>
        private void ResetJump() {
            anim.SetBool("Walk", false);
            canJump = true;
        }

        #endregion

        #region Potion Effects

        
        /// <summary>
        /// Deals with the interaction with different items type in the inventory, allowing the player to "consume" these items
        /// </summary>
        /// <param name="item"></param>
        /// This function is called here to start coroutine, it must be called here as the inv object can be deactivated which would stop the coroutine finishing
        public void Interaction(Item item) {
            switch (item.itemdata.type) {
                //If the weapon is consumed then the damage is dealt to the player health
                case "weapon": {
                    var dmg = item.itemdata.desc.Split(' '); //The dmg is taken directly from the descritpion
                    Damage(int.Parse(dmg[0]));
                    break;
                }
                //Heals the player slightly
                case "food":
                    Heal(1f);
                    break;
                //Runs all the potion effects
                case "potion" when item.itemdata.id == "heal":
                    Heal(10f);
                    break;
                case "potion" when item.itemdata.id == "Jump":
                    StartCoroutine(JumpBoost());
                    break;
                case "potion" when item.itemdata.id == "Regeneration":
                    StartCoroutine(RegenerationBoost());
                    break;
                case "potion" when item.itemdata.id == "Resistance":
                    StartCoroutine(ResistanceBoost());
                    break;
                case "potion" when item.itemdata.id == "Speed":
                    StartCoroutine(SpeedBoost());
                    break;
                case "potion": {
                    if (item.itemdata.id == "Strength") StartCoroutine(StrengthBoost());
                    break;
                }
                default:
                    Debug.Log("No Applicable type found");
                    break;
            }

            eatAnim();
        }

        /// <summary>
        /// Player Animation for eating
        /// </summary>
        private void eatAnim() {
            anim.SetTrigger("Attack");
        }

        /// <summary>
        /// Heals the player by a certain amount
        /// </summary>
        /// <param name="amt"></param>
        private void Heal(float amt) {
            health += amt;

            if (health > 100) health = 100f;
        }


        /// <summary>
        /// Coroutine for the jump boost potion effect
        /// </summary>
        /// <returns></returns>
        private IEnumerator JumpBoost() {
            jump = 10f;
            yield return new WaitForSeconds(10f);
            jump = 5f;
        }

        /// <summary>
        /// Coroutine for the regeneration boost potion effect
        /// </summary>
        /// <returns></returns>
        private IEnumerator RegenerationBoost() {
            regen = 3f;
            yield return new WaitForSeconds(10f);
            regen = 0f;
        }

        /// <summary>
        /// Coroutine for the resistance boost potion effect
        /// </summary>
        /// <returns></returns>
        private IEnumerator ResistanceBoost() {
            resistanceMultiplier = 0.5f;
            yield return new WaitForSeconds(10f);
            resistanceMultiplier = 1f;
        }

        /// <summary>
        /// Coroutine for the speed boost potion effect
        /// </summary>
        /// <returns></returns>
        private IEnumerator SpeedBoost() {
            speed = 3f;
            yield return new WaitForSeconds(10f);
            speed = 1f;
        }

        /// <summary>
        /// Coroutine for the strength boost potion effect
        /// </summary>
        /// <returns></returns>
        private IEnumerator StrengthBoost() {
            strength = 3;
            yield return new WaitForSeconds(10f);
            strength = 1;
        }

        #endregion
    }
}