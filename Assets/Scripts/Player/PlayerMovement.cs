using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float speed = 3;
    [SerializeField] private float jumpForce = 5;
    [SerializeField] private Vector3 groundCheckOffset;
    


    private Vector3 input;
    private bool isMoving;
    private bool isFlying;
    private bool isGrounded;

    private Rigidbody2D rb;
    private PlayerAnimation animations;
    [SerializeField] private SpriteRenderer PlayerSprite;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponentInChildren<PlayerAnimation>();
    }

    private void Update()
    {
        Move();
        CheckGround();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        animations.IsMoving = isMoving;
        animations.IsFlying = IsFlying();
    }

    // я хуй знает почему это не работает, пока что условие для прыжка просто въебал тру, поэтому можно в воздухе прыгать
    private void CheckGround()
    {
        float rayLength = 0.62f;
        
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, rayLength, LayerMask.GetMask("Ground"));
        
        if (hit.collider != null)
        {
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
    }


    private bool IsFlying()
    {
        if (rb.velocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Move()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), 0);
        transform.position += input * speed * Time.deltaTime;
        isMoving = input.x != 0 ? true : false;

        if (isMoving)
        {
            PlayerSprite.flipX = input.x > 0 ? false : true;
        }

        animations.IsMoving = isMoving;
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            animations.Jump();
        }
    }
}
