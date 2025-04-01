using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle,    // ����� �� �����
    Walk,    // ����
    Jump,    // �������
    Attack,  // �������
    Roll     // �������
}

public class PlayerFSM : MonoBehaviour
{
    // ������� ��������� ������
    private PlayerState currentState;

    // ���������� ������
    private Rigidbody2D rb;
    private Animator animator;

    // ��������� ��������
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rollSpeed = 8f;      // �������� �������
    public float rollDuration = 0.5f; // ������������ �������
    private float rollTimer;          // ������ ��� ������������ ������������
    private float moveDirection;      // ����������� �������� ��� �������

    void Start()
    {
        // �������� ����������
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // ������������� ��������� ���������
        ChangeState(PlayerState.Idle);
    }

    void Update()
    {
        // ��������� ���������
        switch (currentState)
        {
            case PlayerState.Idle:
                UpdateIdleState();
                break;

            case PlayerState.Walk:
                UpdateWalkState();
                break;

            case PlayerState.Jump:
                UpdateJumpState();
                break;

            case PlayerState.Attack:
                UpdateAttackState();
                break;

            case PlayerState.Roll:
                UpdateRollState();
                break;
        }
    }

    // ����� ��� ����� ���������
    private void ChangeState(PlayerState newState)
    {
        // ����� �� ����������� ���������
        ExitState(currentState);

        // ��������� ������ ���������
        currentState = newState;

        // ���� � ����� ���������
        EnterState(currentState);
    }

    // ������ ����� � ���������
    private void EnterState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                animator.Play("Idle");
                break;

            case PlayerState.Walk:
                animator.Play("Walk");
                break;

            case PlayerState.Jump:
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                animator.Play("Jump");
                break;

            case PlayerState.Attack:
                animator.Play("Attack");
                break;

            case PlayerState.Roll:
                animator.Play("Roll");
                rollTimer = rollDuration;
                moveDirection = Input.GetAxisRaw("Horizontal") != 0 ?
                    Input.GetAxisRaw("Horizontal") : transform.localScale.x; // ���� ��� �����, ���������� ����������� �������
                rb.linearVelocity = new Vector2(moveDirection * rollSpeed, rb.linearVelocity.y);
                break;
        }
    }

    // ������ ������ �� ���������
    private void ExitState(PlayerState state)
    {
        // ������� ��� ������
        if (state == PlayerState.Roll)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // ������������� �������������� ��������
        }
    }

    // ���������� ���������
    private void UpdateIdleState()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            ChangeState(PlayerState.Walk);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlayerState.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ChangeState(PlayerState.Attack);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) // ������� �� Shift
        {
            ChangeState(PlayerState.Roll);
        }
    }

    private void UpdateWalkState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput == 0)
        {

           
ChangeState(PlayerState.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlayerState.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) // ������� �� Shift
        {
            ChangeState(PlayerState.Roll);
        }
    }

    private void UpdateJumpState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.y == 0) // �������� �����������
        {
            ChangeState(PlayerState.Idle);
        }
    }

    private void UpdateAttackState()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            ChangeState(PlayerState.Idle);
        }
    }

    private void UpdateRollState()
    {
        rollTimer -= Time.deltaTime;

        // ������������ �������� �������
        rb.linearVelocity = new Vector2(moveDirection * rollSpeed, rb.linearVelocity.y);

        // ��������� �������, ����� ������ ��������
        if (rollTimer <= 0)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    // �������� �����������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && currentState == PlayerState.Jump)
        {
            ChangeState(PlayerState.Idle);
        }
    }
}

