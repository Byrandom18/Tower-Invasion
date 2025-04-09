using UnityEngine;

public class PlayerStrongAttack : MonoBehaviour
{
    // Переменные для атаки
    public int strongAttackDamage = 20;
    public float attackRange = 1f;
    public float attackRate = 2f;
    private float nextAttackTime = 0f;

    // Переменные для маны
    public float maxMana = 100f;
    private float currentMana = 100f;
    public float strongAttackManaCost = 20f;

    private Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    private PlayerBlock playerBlock; 


    void Update()
    {
        // Сильная атака с проверкой маны и блока
        if (Time.time >= nextAttackTime && Input.GetKeyDown(KeyCode.Mouse1) &&
            (playerBlock == null || !playerBlock.IsBlocking()))
        {
            if (currentMana >= strongAttackManaCost)
            {
                Attack();
                currentMana -= strongAttackManaCost;
                currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
                nextAttackTime = Time.time + 1f / attackRate;
                Debug.Log($"Мана потрачена: {strongAttackManaCost}. Текущая мана: {currentMana}");
            }
            else
            {
                Debug.Log("Недостаточно маны для сильной атаки!");
            }
        }
    }

    void Attack()
    {
   

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyDamage enemyDamage = enemy.GetComponent<EnemyDamage>();
            if (enemyDamage != null)
            {
                enemyDamage.TakeDamage(strongAttackDamage,transform.position);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red; // Красный для сильной атаки
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void RestoreMana(float amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0f, maxMana);
    }


 
    public float GetManaPercentage()
    {
        return currentMana / maxMana;
    }
}