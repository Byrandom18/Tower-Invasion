using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Максимальное здоровье
    private float currentHealth;   // Текущее здоровье
    private bool isDead = false;   // Флаг смерти

    // Переменные для отбрасывания
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    private Rigidbody2D rb;
    private bool isKnockedBack = false;
    private float knockbackTimer;

    // Переменные для зелий
    public int maxPotions = 3;     // Максимальное количество зелий
    private int currentPotions;    // Текущее количество зелий (начинаем с 0)
    public float potionHealAmount = 30f; // Сколько здоровья восстанавливает зелье

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        currentPotions = 0; // Игрок начинает без зелий
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D не найден на объекте игрока!");
        }
    }

    void Update()
    {
        // Обработка отбрасывания
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.zero;
            }
        }

        // Использование зелья по нажатию клавиши (P)
        if (Input.GetKeyDown(KeyCode.P) && !isDead)
        {
            UsePotion();
        }
    }

    // Получение урона с отбрасыванием
    public void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"Игрок получил {damage} урона. Текущее здоровье: {currentHealth}");

        if (rb != null && !isKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
            rb.linearVelocity = knockbackDirection * knockbackForce;
            isKnockedBack = true;
            knockbackTimer = knockbackDuration;
            animator.SetTrigger("Hurt");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Игрок умер!");
        animator.SetTrigger("Death");

        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerStrongAttack>().enabled = false;
        GetComponent<PlayerBlock>().enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    // Использование зелья
    public void UsePotion()
    {
        if (currentPotions > 0 && currentHealth < maxHealth)
        {
            currentPotions--;
            currentHealth += potionHealAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            Debug.Log($"Использовано зелье. Здоровье: {currentHealth}, Зелий осталось: {currentPotions}");
            animator.SetTrigger("Heal");
        }
        else if (currentPotions <= 0)
        {
            Debug.Log("Нет зелий для использования! Подберите зелье.");
        }
        else
        {
            Debug.Log("Здоровье уже полное!");
        }
    }

    // Подбор зелья
    public void AddPotion()
    {
        if (currentPotions < maxPotions)
        {
            currentPotions++;
            Debug.Log($"Подобрано зелье. Зелий теперь: {currentPotions}");
        }
        else
        {
            Debug.Log("Инвентарь зелий полон!");
        }
    }

    // Восстановление у костра
    public void RestAtBonfire()
    {
        if (isDead) return;

        currentHealth = maxHealth;
        currentPotions = maxPotions; // Костер все еще восстанавливает зелья до максимума
        Debug.Log($"Отдых у костра. Здоровье: {currentHealth}, Зелья: {currentPotions}");
        animator.SetTrigger("Heal");
    }


    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    public int GetCurrentPotions()
    {
        return currentPotions;
    }

    public bool IsAlive()
    {
        return !isDead;
    }
}