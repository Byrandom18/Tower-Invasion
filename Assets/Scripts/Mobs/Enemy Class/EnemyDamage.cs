using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float Damage;
    // скрипт со статами игрока
    public PlayerStats playerStats;
    public Transform player;

    [SerializeField] private bool isBoss = false;

    public float health = 100;


    public ParticleSystem DamageParticles;
    public float particleSpeed = 3f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                player.TakeDamage(Damage);
            }
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
        if (isKnockbackActive && !isBoss) return; // Игнорируем новый урон во время отскока

        // Применяем урон
        health -= damage;

        //particles
        ParticlesLaunch();

        // Эффект визуального удара
        StartCoroutine(FlashRed());

        // Эффект отскока
        if (!isBoss)
        {
            ApplyKnockback(damageSourcePosition);
        }
            

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

    private void ParticlesLaunch()
    {
        // Направление от игрока к врагу (и разворачиваем его)
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Vector3 oppositeDirection = -directionToPlayer; // Направление "от игрока"

        // Настраиваем ParticleSystem
        var main = DamageParticles.main;
        //main.startSpeed = particleSpeed;
        main.startRotation = Mathf.Atan2(oppositeDirection.y, oppositeDirection.x); // Угол в радианах

        // Запускаем частицы
        DamageParticles.Play();
    }


    void Die()
    {
        Destroy(gameObject);
    }
}
