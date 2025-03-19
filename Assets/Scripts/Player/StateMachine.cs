using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Idle,    // Стоит на месте
    Walk,    // Идет
    Jump,    // Прыгает
    Attack,  // Атакует
    Roll     // Кувырок
}

public class PlayerFSM : MonoBehaviour
{
    // Текущее состояние игрока
    private PlayerState currentState;

    // Компоненты игрока
    private Rigidbody2D rb;
    private Animator animator;

    // Параметры движения
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float rollSpeed = 8f;      // Скорость кувырка
    public float rollDuration = 0.5f; // Длительность кувырка
    private float rollTimer;          // Таймер для отслеживания длительности
    private float moveDirection;      // Направление движения для кувырка

    void Start()
    {
        // Получаем компоненты
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Устанавливаем начальное состояние
        ChangeState(PlayerState.Idle);
    }

    void Update()
    {
        // Обработка состояний
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

    // Метод для смены состояния
    private void ChangeState(PlayerState newState)
    {
        // Выход из предыдущего состояния
        ExitState(currentState);

        // Установка нового состояния
        currentState = newState;

        // Вход в новое состояние
        EnterState(currentState);
    }

    // Методы входа в состояние
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
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                animator.Play("Jump");
                break;

            case PlayerState.Attack:
                animator.Play("Attack");
                break;

            case PlayerState.Roll:
                animator.Play("Roll");
                rollTimer = rollDuration;
                moveDirection = Input.GetAxisRaw("Horizontal") != 0 ?
                    Input.GetAxisRaw("Horizontal") : transform.localScale.x; // Если нет ввода, используем направление взгляда
                rb.velocity = new Vector2(moveDirection * rollSpeed, rb.velocity.y);
                break;
        }
    }

    // Методы выхода из состояния
    private void ExitState(PlayerState state)
    {
        // Очистка при выходе
        if (state == PlayerState.Roll)
        {
            rb.velocity = new Vector2(0, rb.velocity.y); // Останавливаем горизонтальное движение
        }
    }

    // Обновление состояний
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
        else if (Input.GetKeyDown(KeyCode.LeftShift)) // Кувырок по Shift
        {
            ChangeState(PlayerState.Roll);
        }
    }

    private void UpdateWalkState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput == 0)
        {

           
ChangeState(PlayerState.Idle);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeState(PlayerState.Jump);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift)) // Кувырок по Shift
        {
            ChangeState(PlayerState.Roll);
        }
    }

    private void UpdateJumpState()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (rb.velocity.y == 0) // Проверка приземления
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

        // Поддерживаем скорость кувырка
        rb.velocity = new Vector2(moveDirection * rollSpeed, rb.velocity.y);

        // Завершаем кувырок, когда таймер истекает
        if (rollTimer <= 0)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    // Проверка приземления
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && currentState == PlayerState.Jump)
        {
            ChangeState(PlayerState.Idle);
        }
    }
}

