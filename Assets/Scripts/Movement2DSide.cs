using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2DSide : MonoBehaviour
{
    Rigidbody2D rb; //player rigidbody
    public float Scale; //scale of the game
    [Range (0,1)]
    public float speed; //speed magnitude
    [Range (0,5)]
    public float jumpHeight; //jump height magnitude
    [Range(0, 10)]
    public float Friction; //stops object on the floor, use 0 for no friction
    bool canJump; //if the object can jump
    public int wallJumps; //times an object can jump off a wall
    int currentWallJumps; //current amount of jumps off walls
    bool grounded; // if the object is on the floor

    bool canInput = true; // global check for if inputs can be done (for stun/knockback)

    Animator anim;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speed *= Scale;
        jumpHeight *= Scale / 2;
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;
        if(canInput) {
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
        //attack
        if (Input.GetMouseButtonDown(0))
        { //fix for controller later
            anim.SetFloat("Attack", 1);
        }
        else anim.SetFloat("Attack", 0);

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
        //enemy contact damage
        else if (collision.gameObject.tag == "Enemy") { 
            // hurt player
        }
        //this depends on context, change to smaller value for enemy vs jumpboost
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
    private void OnTriggerStay2D(Collider2D collision)
    {
        // damages enemy in range (add attack power later)
        if (Input.GetMouseButtonDown(0) && collision.gameObject.tag == "Enemy") {
            //  collision.gameObject.GetComponent<BasicEnemy>().Damage(1);
        }
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
