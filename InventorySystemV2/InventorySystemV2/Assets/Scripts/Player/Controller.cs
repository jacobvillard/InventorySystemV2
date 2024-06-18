using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Player controller
/// </summary>
public class Controller : Singleton<Controller>, IDamageable
{
    #region player variables
    [Header("Player Stats")]
    [SerializeField] float speed = 1f;
    [SerializeField] float jump = 5f;
    [SerializeField] float health = 100f;
    [SerializeField] int strength = 1;

    //Potion effects
    private float regen = 0f, restistanceMultiplier = 1f;
    private static float maxHealth = 100f;

    //Item References
    public Item item;
    public bool removeItem = false;
    public int hotbar = 1;
    public int sphrereRadius = 1;

    //Player variable(s)
    private bool canJump = true;
    #endregion

    #region player components
    [Header("Heatlh Bar")]
    [SerializeField] GameObject healthbarHUD;


    [Header("Inventory Gameobject")]
    [SerializeField] GameObject inventory;




    //Player components
    private SpriteRenderer sprite;
    private Animator anim;
    private Rigidbody2D rb;

    private Collider2D[] objectsInsideArea;
    #endregion

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {

        //Player health
        healthbarHUD.GetComponent<Bar>().UpdateBar(health, maxHealth);
        health += regen * Time.deltaTime;

        //Player controls
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0);
        FlipSprite();

        if (Input.GetKeyDown(KeyCode.Space)) Jump();//Jumping
        if (Input.GetMouseButtonDown(0))//Attacking
        {
            if(item != null && item.itemdata.type == "weapon")
            {
                string[] dmg = item.itemdata.desc.Split(' ');//The dmg is taken directly from the descritpion
                Attack(int.Parse(dmg[0]));
            }
            else
            {
                Attack(1);
            }
           
        }
        if (Input.GetMouseButtonDown(1) && !inventory.activeSelf) RightClick();//Consuming

        if (Input.GetKeyDown(KeyCode.F1))hotbar = 1;//Hotbar keybinds
        else if (Input.GetKeyDown(KeyCode.F2))hotbar = 2;
        else if (Input.GetKeyDown(KeyCode.F3))hotbar = 3;
        else if (Input.GetKeyDown(KeyCode.F4))hotbar = 4;
        else if (Input.GetKeyDown(KeyCode.F5))hotbar = 5;
        else if (Input.GetKeyDown(KeyCode.F6))hotbar = 6;


        //Overlapshere for dealing damage to ene,ies
        if (item == null) sphrereRadius = 1;
        else sphrereRadius = item.itemdata.width;

       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(this.gameObject.transform.GetChild(0).position, sphrereRadius) ;
    }

    #region Flip Sprite
    void FlipSprite()
    {
        if (Input.GetAxis("Horizontal") > 0f)
        {
            sprite.flipX = false;
        }
        else if (Input.GetAxis("Horizontal") < 0f)
        {
            sprite.flipX = true;
        }
    }
    
    #endregion

    #region Jump
    void Jump()
    {
        if (!canJump) return;
        canJump = false;
        anim.SetBool("Walk", true);
        rb.AddForce(Vector2.up * jump, ForceMode2D.Impulse);
        Invoke("resetJump", jump/5);
    }

    void resetJump()
    {
        anim.SetBool("Walk", false);
        canJump = true;
    }

    #endregion

    #region Take Damage
    public void Damage(int dmg)//Take damage to player 
    {
        health -= dmg * restistanceMultiplier;

        if (health <= 0)
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }
    }
    #endregion

    private void Attack(int attackDmg)//Take damage to player 
    {
        Animation attackAnim = this.gameObject.transform.GetChild(0).GetComponent<Animation>();
        if(!canJump) attackAnim.Play("WeaponSlashCircle");
        else if(!sprite.flipX)attackAnim.Play("WeaponSlash");
        else attackAnim.Play("WeaponSlashBackward");

        objectsInsideArea = Physics2D.OverlapCircleAll(gameObject.transform.GetChild(0).position, sphrereRadius);
        foreach (Collider2D IsEnemy in objectsInsideArea)
        {
            if (IsEnemy.gameObject.CompareTag("Enemy"))
            {
                IsEnemy.GetComponent<IDamageable>().Damage(attackDmg * strength);
                Debug.Log("d");
            }

        }


        

    }




    private void RightClick()//Consume item in hand
    {
        if (item == null) return;
        interaction(item);
        eatAnim();
        removeItem = true;
        SpriteRenderer weaponSprite = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        weaponSprite.sprite = null;
        item = null;

        

    }

    #region Potion Effects

    //This function is called here to start coroutine, it must be called here as the inv object can be deacativated which would stop the coroutine finishing
    //It deals with the interaction with different items type in the inventory, allowing the player to "consume" these items
    public void interaction(Item item)
    {
        if (item.itemdata.type == "weapon")//If the weapon is consumed then the damage is dealt to the player health
        {
            string[] dmg = item.itemdata.desc.Split(' ');//The dmg is taken directly from the descritpion
            Damage(int.Parse(dmg[0]));
        }
        else if (item.itemdata.type == "food")//Heals the player slightly
        {
            Heal(1f);
        }
        else if (item.itemdata.type == "potion")//Runs all the potion effects
        {
            if (item.itemdata.id == "heal") Heal(10f);
            else if (item.itemdata.id == "Jump") StartCoroutine(JumpBoost());
            else if (item.itemdata.id == "Regeneration") StartCoroutine(RegenerationBoost());
            else if (item.itemdata.id == "Resistance") StartCoroutine(ResistanceBoost());
            else if (item.itemdata.id == "Speed") StartCoroutine(SpeedBoost());
            else if (item.itemdata.id == "Strength") StartCoroutine(StrengthBoost());
        }
        else
        {
            Debug.Log("No Applicable type found");
        }
        
        eatAnim();
    }

    //Player Animation
    public void eatAnim()
    {
        anim.SetTrigger("Attack");
    }

    //Heal the player by amt
    public void Heal(float amt)
    {
        health += amt;

        if (health > 100) health = 100f;

    }

    //All of the potion effect change a variable which is reverted back after 10 secondsa
    public IEnumerator JumpBoost()
    {
        jump = 10f;
        yield return new WaitForSeconds(10f);
        jump = 5f;
    }

    public IEnumerator RegenerationBoost()
    {
        regen = 3f;
        yield return new WaitForSeconds(10f);
        regen = 0f;
    }
    public IEnumerator ResistanceBoost()
    {
        restistanceMultiplier = 0.5f;
        yield return new WaitForSeconds(10f);
        restistanceMultiplier = 1f;
    }
    public IEnumerator SpeedBoost()
    {

        speed = 3f;
        yield return new WaitForSeconds(10f);
        speed = 1f;
    }
    public IEnumerator StrengthBoost()
    {
        strength = 3;
        yield return new WaitForSeconds(10f);
        strength = 1;
    }


   
    #endregion
}


