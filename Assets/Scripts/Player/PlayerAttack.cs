using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    // Переменные
    [SerializeField] private float attackRange = 0.5f;      // Радиус атаки
    [SerializeField] private float attackRate = 2f;         // Скорость атаки (атаки в секунду)
    [SerializeField] private int attackDamage = 20;         // Урон от атаки
    [SerializeField] private Transform attackPoint;         // Точка начала атаки
    [SerializeField] private LayerMask enemyLayers;         // Слои врагов

    private PlayerAnimation animations;

    private float nextAttackTime = 0f;                      // Время до следующей атаки
    public float manaGainPerAttack = 10f;
    private PlayerBlock playerBlock;
    private PlayerStrongAttack strongAttack;

    void Start()
    {
        animations = GetComponentInChildren<PlayerAnimation>();
        playerBlock = GetComponent<PlayerBlock>();
        strongAttack = GetComponent<PlayerStrongAttack>();


        // Проверка, найден ли компонент сильной атаки
        if (strongAttack == null)
        {
            Debug.LogError("PlayerStrongAttack не найден! Мана не будет восстанавливаться.");
        }
    }
    void Update()
    {
        // Проверка возможности атаки
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Mouse0) &&
        (playerBlock == null || !playerBlock.IsBlocking()))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return; // Игнорировать клик, если курсор над UI

            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }
        
    

    void Attack()
    {
        animations.Attack = true;
        // Обнаружение врагов в радиусе атаки
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Нанесение урона всем попавшим врагам
        foreach (Collider2D enemy in hitEnemies)
        {
            // Предполагается, что у врага есть скрипт Enemy с методом TakeDamage
            enemy.GetComponent<EnemyDamage>().TakeDamage(attackDamage, transform.position);
            
            if (strongAttack != null)
            {
                strongAttack.RestoreMana(manaGainPerAttack);
                Debug.Log($"Мана восстановлена на {manaGainPerAttack}. Текущая мана: {strongAttack.GetManaPercentage() * strongAttack.maxMana}");
            }
        }

      
    }

    void AttackEnd()
    {
        animations.Attack = false;
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
