using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2DSide : MonoBehaviour
{
    Rigidbody2D rb; //player rigidbody
    public float Scale; //scale of the game
    [Range (0,10)]
    public float speed; //speed magnitude
    [Range (0,50)]
    public float jumpHeight; //jump height magnitude
    [Range(0, 10)]
    public float Friction; //stops object on the floor, use 0 for no friction
    bool canJump; //if the object can jump
    public int wallJumps; //times an object can jump off a wall
    int currentWallJumps; //current amount of jumps off walls
    bool grounded; // if the object is on the floor

    bool canInput = true; // global check for if inputs can be done (for stun/knockback)

    GameObject currentTarget; // which plant the player is looking at

    GameObject currentlyHolding = null; // what item the player is holding (watering can = "")
    int currentlyHoldingData; // the data of what item the player currently has (empty when = -1)

    [SerializeField]
    float pickupReach; // how far away can the item be picked up from
    [SerializeField]
    SelectTarget pickupPointer; // what the player is closest to
    // number relative to index in CharacterStats: [water, feeding, pesticide, selling, tilling, planting, harvesting]

    Animator anim;
    SpriteRenderer sr;
    CharacterStats stats;
    public SoundEffectPlayer sounds;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        speed *= Scale;
        jumpHeight *= Scale / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        // check for closest item
        GameObject target = getNearestItem();
        pickupPointer.setTarget(target == null ? currentTarget: target);
        if (canInput) {
            x = Input.GetAxisRaw("Horizontal");
            y = Input.GetAxisRaw("Vertical");
        }
        //jumping
        if (canJump && y > 0)
        {
            rb.drag = Friction;
            canJump = false;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
            anim.SetBool("Jump", true);
            anim.SetBool("Grounded", false);
        }
        //horizontal movement
        if (Mathf.Abs(x) > 0) { 
            rb.velocity = new Vector2(x * speed, rb.velocity.y);
            anim.SetInteger("xDir", (int)Input.GetAxisRaw("Horizontal"));
            //Flip sprite based on input
            if (x > 0) sr.flipX = false;
            else sr.flipX = true;
        }
        else anim.SetInteger("xDir", 0);
        //physics-y stuff
        if (Mathf.Abs(rb.velocity.y) > 0.5f || Mathf.Abs(rb.velocity.x) <= (x * speed) || !grounded) rb.drag = 0;
        else rb.drag = Friction;
        
        // use
        if (canInput && Input.GetMouseButtonDown(0) && currentTarget != null && currentTarget.tag == "Plant")
        {
            if (currentlyHoldingData == 4)
            {
                // planting
                currentTarget.GetComponent<PlantStats>().Sow(currentlyHolding.GetComponent<ItemStats>().seedStats, currentlyHolding);
                Destroy(currentlyHolding); // could do something else with this
                currentlyHolding = null;
                currentlyHoldingData = -1;
            }
            else if (currentlyHolding == null && currentTarget.tag == "Plant")
            {
                // harvesting
                string temp = currentTarget.GetComponent<PlantStats>().Harvest();
                if (!temp.Equals("X"))
                {
                    currentlyHolding = (GameObject)Instantiate(Resources.Load(temp));
                    currentlyHolding.transform.parent = transform;
                }
            }
            else if (currentlyHoldingData != -1)
            {
                // watering, fertilizing, or spraying pesticides
                currentTarget.GetComponent<PlantStats>().UseManager(stats.rates[currentlyHoldingData], currentlyHoldingData, currentlyHolding);
                StartCoroutine(InputDisable(stats.cooldowns[currentlyHoldingData]));
                if(currentlyHoldingData != 0)
                {
                    Destroy(currentlyHolding);
                    currentlyHolding = null;
                    currentlyHoldingData = -1;
                    sounds.play(1);
                }
                else
                {
                    sounds.play(2);
                    currentlyHolding.GetComponent<ItemStats>().isEmpty = true;
                }
            }
        }
        // pickup
        else if (canInput && Input.GetMouseButtonDown(1)) // might need to change the item targeting system to nearest item
        {
            if (target != null)
            {
                // pickup an item
                if (target.name.Equals("Fertilizer"))
                {
                    // get fertilizer
                    if (currentlyHolding != null) currentlyHolding.GetComponent<ItemStats>().Throw(x);
                    currentlyHoldingData = 1;
                    currentlyHolding = (GameObject)Instantiate(Resources.Load("FertilizerItem"));
                    currentlyHolding.transform.parent = transform;
                    // set currentlyHolding to object
                    StartCoroutine(InputDisable(stats.cooldowns[1]));
                    sounds.play(0);

                }
                else if (target.name.Equals("Pesticide"))
                {
                    // get pesticide
                    if (currentlyHolding != null) currentlyHolding.GetComponent<ItemStats>().Throw(x);
                    currentlyHoldingData = 2;
                    currentlyHolding = (GameObject)Instantiate(Resources.Load("PesticideItem"));
                    currentlyHolding.transform.parent = transform;
                    // set currentlyHolding to object
                    StartCoroutine(InputDisable(stats.cooldowns[2]));
                    sounds.play(0);
                }
                else if (target.name.Equals("Well"))
                {
                    if (currentlyHolding != null && currentlyHolding.name.Contains("Watering Can"))
                    {
                        // fill watering can
                        currentlyHoldingData = 0;
                        currentlyHolding.GetComponent<ItemStats>().isEmpty = false;
                        StartCoroutine(InputDisable(stats.cooldowns[0]));
                        sounds.play(2);
                    }
                }
                else
                {
                    if (currentlyHolding != null)
                    {
                        // throw the item you're holding
                        currentlyHolding.GetComponent<ItemStats>().Throw(x);
                        currentlyHolding = null;
                    }
                    // get new item
                    currentlyHolding = target;
                    target.transform.parent = transform;
                    target.GetComponent<Rigidbody2D>().gravityScale = 0;
                    currentlyHoldingData = target.GetComponent<ItemStats>().type;
                    sounds.play(0);
                }
            }
            else if(currentlyHolding != null)
            {
                // throw item
                currentlyHoldingData = -1;
                currentlyHolding.GetComponent<ItemStats>().Throw(x);
                currentlyHolding = null;
                sounds.play(1);
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //tags
        if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
            currentWallJumps = 0;
            grounded = true;
            anim.SetBool("Jump", false);
            anim.SetBool("Grounded", true);
        }
        //controls wall jumping 
        else if (currentWallJumps < wallJumps)
        {
            currentWallJumps++;
            canJump = true;
            anim.SetBool("Wall", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") grounded = false;
        else if (collision.gameObject.tag != "Enemy")
        {
            anim.SetBool("Wall", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentTarget = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (currentTarget == collision.gameObject) currentTarget = null;
    }
    GameObject getNearestItem() 
    {
        float distance = 300;
        GameObject closest = null;
        // find closest item if within reach
        foreach (GameObject i in GameObject.FindGameObjectsWithTag("Item")) 
        {
            if (currentlyHolding == i) continue;
            float temp = Mathf.Abs(transform.position.magnitude - i.transform.position.magnitude);
            if (distance > temp && temp < pickupReach)
            {
                closest = i;
                distance = temp;
            }
        }
        return closest;
    }
    IEnumerator InputDisable(float seconds, bool hit = false)
    {
        // disables inputs for seconds and disables enemy hit  if hit = true
        canInput = false;
        if (hit) gameObject.layer = 3;
        yield return new WaitForSeconds(seconds);
        canInput = true;
        if (hit)
        {
            yield return new WaitForSeconds(seconds);
            gameObject.layer = 0;
        }
    }
}
