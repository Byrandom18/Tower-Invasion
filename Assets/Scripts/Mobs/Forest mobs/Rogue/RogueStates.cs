using UnityEngine;

public class RogueStates : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;

    [Header("References")]
    [SerializeField] private PlayerStats playerStats; // Перетащите игрока в инспекторе
    [SerializeField] private EnemyDamage enemyDamage; // Перетащите компонент с уроном
    [SerializeField] private RogueAnimations animations;
    [SerializeField] private EnemyClass enemy;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask playerLayer;


    private float damage;
    private Vector2 attackDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        // Проверяем и получаем компоненты
        if (enemyDamage == null)
            enemyDamage = GetComponent<EnemyDamage>();

        if (animations == null)
            animations = GetComponent<RogueAnimations>();

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

    public void AttackClosing()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.AddForce(new Vector2(direction.x * 150, 0));
    }

    public void ChangeState()
    {
        enemy.state = 2;
        animations.IsAttack = false;
        animations.Attack();

    }


    public void Death()
    {

    }
}
