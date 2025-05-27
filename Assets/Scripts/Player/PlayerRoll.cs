using UnityEngine;

public class PlayerRoll : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer; // Для управления flipX
    public float rollSpeed = 5f; // Скорость кувырка
    public float rollDuration = 0.5f; // Длительность кувырка
    private float rollTimer; // Таймер
    private bool isRolling = false; // Флаг кувырка
    private Animator animator; // Для управления анимацией
    private int originalLayer; // Сохраняем исходный слой персонажа
    private int ignoreLayer; // Слой для неуязвимости
    private float rollDirection; // Направление кувырка

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalLayer = gameObject.layer; // Сохраняем исходный слой
        ignoreLayer = LayerMask.NameToLayer("IgnoreCollisions"); // Указываем слой неуязвимости
    }

    void Update()
    {
        // Проверка ввода для кувырка 
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
        {
            StartRoll();
        }

        // Обновляем таймер кувырка
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0)
            {
                StopRoll();
            }
        }
    }

    void FixedUpdate()
    {
        // Применяем скорость кувырка в FixedUpdate для стабильной физики
        if (isRolling)
        {
            rb.linearVelocity = new Vector2(rollDirection * rollSpeed, rb.linearVelocity.y);
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
        rollDirection = Input.GetAxisRaw("Horizontal"); // -1 (влево), 1 (вправо), 0 (нет ввода)

        // Если нет ввода, используем текущее направление персонажа на основе flipX
        if (rollDirection == 0)
        {
            rollDirection = spriteRenderer.flipX ? -1 : 1; // Влево (-1) если flipX = true, иначе вправо (1)
        }

        // Поворачиваем спрайт в сторону кувырка, если ввод изменил направление
        if (rollDirection != 0)
        {
            spriteRenderer.flipX = rollDirection < 0; // flipX = true для влево, false для вправо
        }
    }

    void StopRoll()
    {
        isRolling = false;
        animator.SetBool("isRolling", false); // Остановка анимации
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Останавливаем горизонтальное движение

        // Возвращаем исходный слой
        gameObject.layer = originalLayer;
    }
}