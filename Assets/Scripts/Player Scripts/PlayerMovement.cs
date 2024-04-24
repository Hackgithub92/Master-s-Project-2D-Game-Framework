using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D ground;
    private Animator anim;
    private SpriteRenderer sprite;
    [SerializeField] private LayerMask jumpableGround;

    private float XDirection = 0f;
    [SerializeField] private float speed = 20f;
    [SerializeField] private float jumpForce = 12f;

    private enum MovementState { idle, running, jumping, falling}


    //instantiate variables.
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    //Update x and y velocities in real time and check for grounding to
    //prevent double jumping. Call animations for movements.
    void Update()
    {
        //Value for Left or Right movement.
        XDirection = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(XDirection * speed, rb.velocity.y);
        
        //Recognize a "W" key press as a jump.
        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimationState();

    }

    //Set all animation states for each invidual movement direction.
    private void UpdateAnimationState()
    {
        MovementState state;

        //Check for running to trigger running animation.
        if (XDirection > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (XDirection < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        //Check for Jumping.
        if (rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    //Check for Player Grounding. This prevents the player
    //from being able to jump infinitely.
    private bool IsGrounded()
    {
        return Physics2D.BoxCast(ground.bounds.center, ground.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
