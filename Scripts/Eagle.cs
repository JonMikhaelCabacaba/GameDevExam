using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Eagle : Enemies
{

    public AIPath pathfinder; 

    private Collider2D col;
    private Rigidbody2D rb;
    //private Animator anim;

   

    protected override void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        // anim = GetComponent<Animator>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(pathfinder.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);

        }
        else if(pathfinder.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
