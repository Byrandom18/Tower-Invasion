using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Переменные
    [SerializeField] private float attackRange = 0.5f;      // Радиус атаки
    [SerializeField] private float attackRate = 2f;         // Скорость атаки (атаки в секунду)
    [SerializeField] private int attackDamage = 20;         // Урон от атаки
    [SerializeField] private Transform attackPoint;         // Точка начала атаки
    [SerializeField] private LayerMask enemyLayers;         // Слои врагов

    private float nextAttackTime = 0f;                      // Время до следующей атаки
    public float manaGainPerAttack = 10f;
    private PlayerBlock playerBlock;
    private PlayerStrongAttack strongAttack;
    void Update()
    {
        // Проверка возможности атаки
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Mouse0) &&
                   (playerBlock == null || !playerBlock.IsBlocking()))
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
        
    

    void Attack()
    {
        // Обнаружение врагов в радиусе атаки
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Нанесение урона всем попавшим врагам
        foreach (Collider2D enemy in hitEnemies)
        {
            // Предполагается, что у врага есть скрипт Enemy с методом TakeDamage
            enemy.GetComponent<EnemyDamage>().TakeDamage(attackDamage, transform.position);
        }
        if (strongAttack != null)
        {
            strongAttack.RestoreMana(manaGainPerAttack);
        }
    }

    // Визуализация радиуса атаки в редакторе (опционально)
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
