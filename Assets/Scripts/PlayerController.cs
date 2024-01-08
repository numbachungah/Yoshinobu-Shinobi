using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;    // Adjust this to control the player's maximum movement speed.
    public float acceleration = 8f; // Adjust this to control the acceleration rate.
    public float deceleration = 7f; // Adjust this to control the deceleration rate.
    public float jumpForce = 10f;    // Adjust this to control the jump force.

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isMoving;
    private bool isOnGround;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = Vector2.zero;
        isMoving = false;
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 inputDirection = new Vector2(horizontalInput, 0).normalized;
        anim.SetBool("moving", isMoving);

        // If the player is providing input, accelerate.
        if (inputDirection.magnitude > 0)
        {
            isMoving = true;

            movement = Vector2.Lerp(movement, inputDirection * moveSpeed, Time.deltaTime * acceleration);
        }
        // If no input is provided, decelerate.
        else
        {
            isMoving = false;
            movement = Vector2.Lerp(movement, Vector2.zero, Time.deltaTime * deceleration);
        }

        // Jumping
        if (Input.GetButtonDown("Jump") && isOnGround)
        {
            // Add upward force for jumping.
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isOnGround = false;
        }

        // Move the player using Rigidbody2D.
        rb.velocity = new Vector2(movement.x, rb.velocity.y);

        // Optionally, you can flip the player sprite based on movement direction.
        if (movement.x > 0)
        {
            transform.localScale = new Vector3(0.59f, 0.6f, 1f);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector3(-0.59f, 0.6f, 1f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("jumpT"))
        {
            isOnGround = true;
        }
    }
}
