using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    private Vector2 input;
    private bool isMoving;
    private bool isGrounded;
    private Rigidbody2D rb;
    private PlayerAnimation animations;
    [SerializeField] private SpriteRenderer playerSprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animations = GetComponentInChildren<PlayerAnimation>();
    }

    void Update()
    {
        // Проверка ввода для прыжка должна быть в Update, а не в FixedUpdate
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void FixedUpdate()
    {
        CheckGround();
        Move();
        UpdateAnimations();
    }

    private void CheckGround()
    {

        // Используем Raycast для проверки земли
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        float rayLength = 10.6f;
        //Debug.Log(isGrounded);
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
        input = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
        rb.velocity = new Vector2(input.x * speed, rb.velocity.y);

        isMoving = Mathf.Abs(input.x) > 0;

        if (isMoving)
        {
            playerSprite.flipX = input.x < 0;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0); // Сбрасываем вертикальную скорость перед прыжком
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animations.Jump();
    }

    private void UpdateAnimations()
    {
        animations.IsMoving = isMoving;
        animations.IsFlying = rb.velocity.y != 0 && !isGrounded;
    }

    // Для отладки в редакторе
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * groundCheckDistance);
    }
}
