using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HogStates : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    public int state = 0;
    [SerializeField] private Transform EnemySprite;
    private bool isMoving = false;
    private PlayerStats playerStats;
    //public MonoBehaviour animationsScript;
    private HogAnimations animations;

    //[SerializeField] private float attackCooldown = 4;
    //private float nextAttackTime = 0f;
    //[SerializeField] private float attackRange = 1f;

    //private bool isIdle = false;
    public EnemyDamage enemyDamage;
    //private float idleDuration = 1f;
    //private bool isPreparating = false;
    //[SerializeField] private float preparationDuration = 3f;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private LayerMask playerLayer; // Какие слои атаковать
    //private bool canAttack = true;
    //private bool isChasing = false;
    private bool alive = true;
    [SerializeField] private ParticleSystem explosiveParticles;

    private float damage = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        enemyDamage = GetComponent<EnemyDamage>();
        animations = GetComponentInChildren<HogAnimations>();

        damage = enemyDamage.Damage;
    }


    private void Update()
    {
        animations.IsMoving = isMoving;
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
                if (distanceX < 0)
                {
                    distanceX = -distanceX;
                }
                if (transform.position.x > playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта
                    isMoving = true;
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y); // Движение влево
                }
                else if (transform.position.x < playerTransform.position.x)
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
                if (distanceX < 0.05f)
                {
                    state = 2;
                }



                break;
            //attack
            case 2:
                if (alive)
                {
                    explosiveParticles.Play();
                }
                Attack();
                state = 3;
                break;
            //death
            case 3:
                animations.Death();
                isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Остановка
                break;
        }
    }

    
    private void Attack()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);

        if (hit != null)
        {
            playerStats = hit.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage*3);
                
            }
        }
    }



    
}
