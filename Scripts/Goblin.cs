using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemies
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerlayer;
    [SerializeField] private LayerMask ground;
    private float cooldownTimer = Mathf.Infinity;

    [SerializeField] private float leftCap;
    [SerializeField] private float rightCap;
    // Start is called before the first frame update

    private bool facingLeft = false;

    [SerializeField] private float walklength = 10f;
    [SerializeField] private float walkheight = 15f;
    //[SerializeField] private LayerMask ground;

    new private Animator anim;
    private Health playerHealth;
    //public PlayerController playerController;

    private Collider2D col;
    private Rigidbody2D rb;
    protected override void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        base.Start();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
        // playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        Move();

        if (PlayerInsight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }



    }
    private void Move()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftCap)
            {
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }
                if (col.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(-walklength, walkheight);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightCap)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }
                if (col.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(walklength, walkheight);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

    private bool PlayerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerlayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
