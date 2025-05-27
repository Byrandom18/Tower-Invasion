using UnityEngine;

public class SlimeStates : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private PlayerStats playerStats; // Перетащите игрока в инспекторе
    [SerializeField] private EnemyDamage enemyDamage; // Перетащите компонент с уроном
    [SerializeField] private SlimeAnimations animations;
    [SerializeField] private EnemyClass enemy;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask playerLayer;

    private DropSystem dropSystem;
    [SerializeField] private int rarity = 0;

    private float damage;
    private Vector2 attackDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Проверяем и получаем компоненты
        if (enemyDamage == null)
            enemyDamage = GetComponent<EnemyDamage>();

        if (animations == null)
            animations = GetComponent<SlimeAnimations>();

        if (enemy == null)
            enemy = GetComponent<EnemyClass>();

        // Проверяем, что компоненты найдены
        if (enemyDamage == null)
            Debug.LogError("EnemyDamage component not found!", this);
        else
            damage = enemyDamage.Damage;

        if (playerStats == null)
            Debug.LogError("PlayerStats reference not set!", this);
    }

    

    public void AttackState()
    {
        attackDirection = transform.right * Mathf.Sign(transform.localScale.x);
        

        RaycastHit2D hit = Physics2D.Raycast(transform.position, attackDirection, attackRange, playerLayer);
        if (hit.collider != null)
        {
            playerStats.TakeDamage(damage);
        }
        
    }

    


    public void ChangeState()
    {
        enemy.state = 2;
        animations.IsAttack = false;
        animations.Attack();
        
    }


    public void DeathState()
    {
        int randomValue = Random.Range(0, 101);
        if (randomValue < rarity)
        {
            dropSystem.SpawnPrefab(1);
            dropSystem.SpawnRandomCollectiblePrefab();
        }
    }
}
