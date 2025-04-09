using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage;
    // скрипт со статами игрока
    public PlayerStats playerStats;
    public int health = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStats.TakeDamage(damage);
        }
    }


    private Color originalColor;
    private Coroutine resetColorCoroutine;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float flashDuration = 0.1f;

    private Rigidbody2D rb;
    private bool isKnockbackActive;
    private Vector2 knockbackDirection;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        if (isKnockbackActive) return; // Игнорируем новый урон во время отскока

        // Применяем урон
        health -= damage;

        // Эффект визуального удара
        StartCoroutine(FlashRed());

        // Эффект отскока
        ApplyKnockback(damageSourcePosition);

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void ApplyKnockback(Vector2 damageSourcePosition)
    {
        // Вычисляем направление отскока (от источника урона)
        knockbackDirection = (Vector2)transform.position - damageSourcePosition;
        knockbackDirection = knockbackDirection.normalized * knockbackForce;

        // Применяем силу отскока
        rb.linearVelocity = Vector2.zero; // Сбрасываем текущую скорость
        rb.AddForce(knockbackDirection, ForceMode2D.Impulse);

        // Запускаем таймер отскока
        isKnockbackActive = true;
        Invoke(nameof(ResetKnockback), knockbackDuration);
    }

    private void ResetKnockback()
    {
        isKnockbackActive = false;
        rb.linearVelocity = Vector2.zero; // Останавливаем моба после отскока
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
