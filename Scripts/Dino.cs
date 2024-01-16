using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : Enemies
{

   [SerializeField] private float leftCap;
   [SerializeField] private float rightCap;
    // Start is called before the first frame update

    [SerializeField] private float walklength = 10f;
    [SerializeField] private float walkheight = 15f;
    [SerializeField] private LayerMask ground;

    private Collider2D col;
    private Rigidbody2D rb;
    //private Animator anim;

    private bool facingLeft = true;
    
    protected override void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();
        base.Start();
    }


    // Update is called once per frame
    private void Update()
    {
        Move();  
        
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
}
