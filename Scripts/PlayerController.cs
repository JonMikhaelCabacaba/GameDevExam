using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D body;
    private Animator animator;

    //FSM
    private enum State { idle, running, jumping, falling, hurt, climb, attack, crouch, slide}
    private State state = State.idle;
    private Collider2D col;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float Speed = 6f;
    [SerializeField] private float jumpforce = 15f;

    [SerializeField] private int gems = 0;
    [SerializeField] private int key = 0;
    //[SerializeField] private int treasure = 0;

    [SerializeField] private int health;
    [SerializeField] private Text healthAmount;

    [SerializeField] private float hurtforce = 15f;
    // [SerializeField] private float moveSpeed = 5f;

    private bool isAttacking = false;
    private float attackCooldown = 2f; // Adjust the cooldown time as needed
    private float currentAttackCooldown = 0.0f;


    [SerializeField] private Text gemsText;
    [SerializeField] private Text keyText;
    [SerializeField] private GameObject additionalCollectable;
    [SerializeField] private GameObject additionalKeyObjects;

    [HideInInspector] public bool canClimb = false;
    [HideInInspector] public bool bottomLadder = false;
    [HideInInspector] public bool topLadder = false;
    public Ladder ladder;
    private float naturalGravity;
    [SerializeField] float climbSpeed = 3f;

    public Text txtToDisplay;
    public DoorController DC;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        naturalGravity = body.gravityScale;
        healthAmount.text = health.ToString();

        additionalCollectable.SetActive(false);
        additionalKeyObjects.SetActive(false);
    }

    private void Update()
    {
        if(state == State.climb)
        {
            Climb();
        }
        else if (state != State.hurt)
        {
            Controls();
        }
        if (currentAttackCooldown > 0)
        {
            currentAttackCooldown -= Time.deltaTime;
        }
        Controls();
        AnimationState();
        animator.SetInteger("state", (int)state); //sets animation based on enumerator state
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            gems += 1;
            gemsText.text = gems.ToString();
        }
        if (collision.tag == "Key")
        {
            Destroy(collision.gameObject);
            key += 1;
            keyText.text = key.ToString();
            DC.gotKey = true;
            txtToDisplay.gameObject.SetActive(true);
            txtToDisplay.text = "Key Acquired";
        }
        if (collision.tag == "Door")
        {
            if (DC.doorState == DoorController.DoorState.Closed && DC.gotKey)
            {
                txtToDisplay.gameObject.SetActive(true);
                txtToDisplay.text = "Press 'E' to Open";
            }
            else if (DC.doorState == DoorController.DoorState.Opened)
            {
                txtToDisplay.gameObject.SetActive(true);
                txtToDisplay.text = "Press 'E' to Close";
            }
            else if (DC.doorState == DoorController.DoorState.Jammed)
            {
                txtToDisplay.gameObject.SetActive(true);
                txtToDisplay.text = "Needs Key";
            }
        }
        if (gems >= 1) //pwede dito yun methid na totalGems
        {
            // Activate the additional collectable GameObject
            if (additionalCollectable != null)
            {
                additionalCollectable.SetActive(true);
                additionalKeyObjects.SetActive(true);
            }
        }
        
    }
    private void TotalGems()
    { 

    }

    private void OnCollisionEnter2D(Collision2D enemy)
    {
        if(enemy.gameObject.tag == "DinoEnemy")
        {
            Dino dino = enemy.gameObject.GetComponent<Dino>();
            if (state == State.falling)
            {
                dino.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                dino.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "SkeletonEnemy")
        {
            Skeleton skel = enemy.gameObject.GetComponent<Skeleton>();
            if (state == State.falling)
            {
                skel.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                skel.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "EyeEnemy")
        {
            EyeEnemy eye = enemy.gameObject.GetComponent<EyeEnemy>();
            if (state == State.falling)
            {
                eye.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                eye.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "MushroomEnemy")
        {
            Mushroom mush = enemy.gameObject.GetComponent<Mushroom>();
            if (state == State.falling)
            {
                mush.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                mush.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "GoblinEnemy")
        {
            Goblin gob = enemy.gameObject.GetComponent<Goblin>();
            if (state == State.falling)
            {
                gob.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                gob.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "EagleEnemy")
        {
            EnemyAI eagle = enemy.gameObject.GetComponent<EnemyAI>();
            if (state == State.falling)
            {
                eagle.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                eagle.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "OpossumEnemy")
        {
            Opossum opossum = enemy.gameObject.GetComponent<Opossum>();
            if (state == State.falling)
            {
                opossum.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                opossum.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }

        if (enemy.gameObject.tag == "BearEnemy")
        {
            Bear bear = enemy.gameObject.GetComponent<Bear>();
            if (state == State.falling )
            {
                bear.JumpedOn();
                Jump();
            }
            else if (state == State.attack)
            {
                bear.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
        if (enemy.gameObject.tag == "FrogEnemy")
        {
            Frog frog = enemy.gameObject.GetComponent<Frog>();
            if (state == State.falling)
            {
                frog.JumpedOn();
                Jump();
            }
            else if(state == State.attack)
            {
                frog.JumpedOn();
            }
            else
            {
                state = State.hurt;
                HandleHealth();
                if (enemy.gameObject.transform.position.x > transform.position.x)
                {
                    //Enemy is to the right it can be damage and move left
                    body.velocity = new Vector2(-hurtforce, body.velocity.y);
                }
                else
                {
                    //Enemy is to the left it can be damage and move right
                    body.velocity = new Vector2(hurtforce, body.velocity.y);
                }

            }
        }
    }

    private void HandleHealth()
    {
        health -= 1;
        healthAmount.text = health.ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void Controls()
    {
        float hDirection = Input.GetAxis("Horizontal");

        if(canClimb && Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            state = State.climb;
            body.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            transform.position = new Vector3(ladder.transform.position.x, body.position.y);
            body.gravityScale = 0f;
        }
        //jump
        if (Input.GetButtonDown("Jump") && col.IsTouchingLayers(ground))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.H) && !isAttacking && currentAttackCooldown <= 0)
        { //Hindi pa sure
            isAttacking = true;
            state = State.attack;
        }
        else if (isAttacking)
        { //Hindi pa sure
            // The attack logic goes here
            // For example, damage or destroy the enemy
            // Once the attack logic is done, set isAttacking to false
            // You may also play an attack animation here
            isAttacking = false;
            currentAttackCooldown = attackCooldown;
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            state = State.crouch;
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            state = State.slide;
        }
        //right
        else if (hDirection > 0)
        {
            body.velocity = new Vector2(5, body.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }

        //left
        else if (hDirection < 0)
        {
            body.velocity = new Vector2(-5, body.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
    }
    private void Climb()
    {
        if (Input.GetButtonDown("Jump"))
        {
            body.constraints = RigidbodyConstraints2D.FreezeRotation;
            canClimb = false;
            body.gravityScale = naturalGravity;
            animator.speed = 1f;
            Jump();
            return;
            
        }
        float vDirection = Input.GetAxis("Vertical");

        if(vDirection > .1f && !topLadder)
        {
            body.velocity = new Vector2(0f, vDirection * climbSpeed);
            animator.speed = 1f;
        }
        else if (vDirection < -.1f && !bottomLadder)
        {
            body.velocity = new Vector2(0f, vDirection * climbSpeed);
            animator.speed = 1f;
        }
        else
        {
            animator.speed = 0f;
            body.velocity = Vector2.zero;
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpforce);
        state = State.jumping;
    }
    private void AnimationState()
    {
        if (state == State.climb) 
        {

        }

        else if(state == State.attack)
        { //Hindi pa sure
            if (Mathf.Abs(body.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (state == State.crouch)
        {

        }
        else if (state == State.slide)
        {
            //Hindi pa sure
            if (Mathf.Abs(body.velocity.x) > .1f)
            {
                state = State.slide;
            }
            else
            {
                state = State.idle;
            }
        }

        else if (state == State.jumping)
        {
            if (body.velocity.y < .1f)
            {
                state = State.falling;
            }
        }
        else if (state == State.falling)
        {
            if (col.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(body.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (Mathf.Abs(body.velocity.x) > 2f)
        {
            state = State.running;
        }
        else
        {
            state = State.idle;
        }
    }

}
