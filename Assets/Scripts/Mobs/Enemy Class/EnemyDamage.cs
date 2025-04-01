using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage;
    // скрипт со статами игрока
    public PlayerStats playerStats;
    [SerializeField] private int health = 100;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void TakeDamage(int damage)
    {
        // Отменяем предыдущую корутину сброса цвета
        if (resetColorCoroutine != null)
        {
            StopCoroutine(resetColorCoroutine);
        }

        // Применяем урон
        health -= damage;

        // Устанавливаем красный цвет
        spriteRenderer.color = Color.red;

        // Запускаем новую корутину сброса цвета
        resetColorCoroutine = StartCoroutine(ResetColor(0.1f));

        if (health <= 0)
        {
            Die();
        }
    }

    private IEnumerator ResetColor(float delay)
    {
        yield return new WaitForSeconds(delay);
        spriteRenderer.color = originalColor;
        resetColorCoroutine = null;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
