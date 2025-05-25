using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DistanceMob : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    public int state = 0;
    [SerializeField] private Transform EnemySprite;
    private bool isMoving = false;

    //public MonoBehaviour animationsScript;
    private SpiritAnimations animations;

    [SerializeField] private float attackCooldown = 4;
    private float nextAttackTime = 0f;
    [SerializeField] private float attackRange = 1f;

    //private bool isIdle = false;
    public EnemyDamage enemyDamage;
    //private float idleDuration = 1f;
    //private bool isPreparating = false;
    //[SerializeField] private float preparationDuration = 3f;
    [SerializeField] private float chaseDistance;

    private bool canAttack = true;
    //private bool isChasing = false;

    public GameObject projectilePrefab;
    
    public float projectileSpeed = 8f;
    public float projectileLifetime = 5f;

    private float damage = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        enemyDamage = GetComponent<EnemyDamage>();
        animations = GetComponentInChildren<SpiritAnimations>();

        damage = enemyDamage.Damage;
    }


    private void Update()
    {
        //animations.IsMoving = isMoving;
        switch (state)
        {
            //idle
            case 0:
                isMoving = false;

                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                {
                    state = 1;
                }

                break;
            //chase
            case 1:
                float distanceX = playerTransform.position.x - transform.position.x;

                if (transform.position.x > playerTransform.position.x && distanceX < -attackRange)
                {
                    EnemySprite.transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта
                    isMoving = true;
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y); // Движение влево
                }
                else if (transform.position.x < playerTransform.position.x && distanceX > attackRange)
                {
                    EnemySprite.transform.localScale = new Vector3(1, 1, 1); // Разворот спрайта
                    isMoving = true;
                    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y); // Движение вправо
                }
                else
                {
                    isMoving = false;
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Остановка
                }


                // attack call
                if (distanceX < 0)
                {
                    distanceX = - distanceX;
                }
                if (distanceX <= attackRange && canAttack)
                {
                    state = 2;
                }
                
                break;
            //attack
            case 2:
                if (canAttack)
                {
                    StartCoroutine(AttackCD());
                }
                if (transform.position.x > playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта
                }
                else if (transform.position.x < playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(1, 1, 1); // Разворот спрайта
                }
                isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Остановка
                animations.IsAttack = true;
                animations.Attack();
                break;
            //death
            case 3:
                animations.Death();
                isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Остановка
                break;
        }
    }

    public void ChangeState()
    {
        state = 1;
        animations.IsAttack = false;
        animations.Attack();

    }


    public void Projectile()
    {
        {
            // Проверяем, назначен ли префаб
            if (projectilePrefab == null)
            {
                Debug.LogError("Projectile prefab is not assigned in DistanceMob!");
                return;
            }

            Vector2 direction = (player.position - transform.position).normalized;

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Projectile projectileScript = projectile.GetComponent<Projectile>();

            // Проверяем, получили ли компонент
            if (projectileScript == null)
            {
                Debug.LogError("Projectile component not found on the projectile prefab!");
                Destroy(projectile); // Уничтожаем снаряд, так как он нерабочий
                return;
            }

            // Устанавливаем параметры
            projectileScript.speed = projectileSpeed;
            projectileScript.lifetime = projectileLifetime;
            projectileScript.damage = damage;

            // Направление устанавливаем последним
            projectileScript.SetDirection(direction);
        }
    }


    IEnumerator AttackCD()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

}
