using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rollSpeed = 5f; // Скорость кувырка
    public float rollDuration = 0.5f; // Длительность кувырка
    private float rollTimer; // Таймер
    private bool isRolling = false; // Флаг кувырка
    private Animator animator; // Для управления анимацией
    private int originalLayer; // Сохраняем исходный слой персонажа
    private int ignoreLayer; // Слой для неуязвимости

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalLayer = gameObject.layer; // Сохраняем исходный слой
        ignoreLayer = LayerMask.NameToLayer("IgnoreCollisions"); // Указываем слой неуязвимости
    }

    void Update()
    {
        // Проверка ввода для кувырка (например, клавиша "Space")
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
        {
            StartRoll();
        }

        // Обработка кувырка
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                StopRoll();
            }
        }
    }

    void StartRoll()
    {
        isRolling = true;
        rollTimer = rollDuration;
        animator.SetBool("isRolling", true); // Запуск анимации кувырка

        // Делаем персонажа неуязвимым, меняя слой
        gameObject.layer = ignoreLayer;

        // Определяем направление кувырка по вводу
        float direction = Input.GetAxisRaw("Horizontal"); // -1 (влево), 1 (вправо), 0 (нет ввода)

        // Если нет ввода, используем текущее направление персонажа
        if (direction == 0)
        {
            direction = transform.localScale.x > 0 ? 1 : -1; // Вправо, если смотрит вправо, иначе влево
        }

        // Применяем скорость в нужном направлении
        rb.velocity = new Vector2(direction * rollSpeed, rb.velocity.y);

        // Поворачиваем персонажа в сторону кувырка (если нужно)
        if (direction != 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
        }
    }

    void StopRoll()
    {
        isRolling = false;
        animator.SetBool("isRolling", false); // Остановка анимации
        rb.velocity = new Vector2(0, rb.velocity.y); // Останавливаем горизонтальное движение

        // Возвращаем исходный слой
        gameObject.layer = originalLayer;
    }
}

