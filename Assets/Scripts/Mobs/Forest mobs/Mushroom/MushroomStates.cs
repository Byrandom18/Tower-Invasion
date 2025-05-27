using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MushroomStates : MonoBehaviour
{
    private Rigidbody2D rb;
    //[SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    public int state = 0;
    [SerializeField] private Transform EnemySprite;
    //private bool isMoving = false;
    private PlayerStats playerStats;
    //public MonoBehaviour animationsScript;
    private MushroomAnimations animations;

    [SerializeField] private float attackCooldown = 4;
    //private float nextAttackTime = 0f;
    [SerializeField] private float attackRange = 2f;

    //private bool isIdle = false;
    public EnemyDamage enemyDamage;
    //private float idleDuration = 1f;
    //private bool isPreparating = false;
    //[SerializeField] private float preparationDuration = 3f;
    [SerializeField] private float chaseDistance;
    [SerializeField] private float attackRadius = 2f;
    [SerializeField] private LayerMask playerLayer; // Какие слои атаковать
    private bool canAttack = true;
    //private bool isChasing = false;
    //private bool alive = true;
    [SerializeField] private ParticleSystem explosiveParticles;
    [SerializeField] private DropSystem dropSystem; // Перетащите в инспекторе
    [SerializeField] private int rarity = 0;
    private float damage = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        enemyDamage = GetComponent<EnemyDamage>();
        animations = GetComponentInChildren<MushroomAnimations>();
        dropSystem = GetComponent<DropSystem>();
        damage = enemyDamage.Damage;
    }


    private void Update()
    {
        //animations.IsMoving = isMoving;
        switch (state)
        {
            //idle
            case 0:
                //isMoving = false;

                if (Vector2.Distance(transform.position, playerTransform.position) < attackRange)
                {
                    state = 2;
                }

                break;
            //chase
            case 1:
                //float distanceX = playerTransform.position.x - transform.position.x;
                //if (distanceX < 0)
                //{
                //    distanceX = -distanceX;
                //}
                //if (transform.position.x > playerTransform.position.x)
                //{
                //    EnemySprite.transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта
                //    //isMoving = true;
                //    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y); // Движение влево
                //}
                //else if (transform.position.x < playerTransform.position.x)
                //{
                //    EnemySprite.transform.localScale = new Vector3(1, 1, 1); // Разворот спрайта
                //    //isMoving = true;
                //    rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y); // Движение вправо
                //}
                //else
                //{
                //    //isMoving = false;
                //    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Остановка
                //}
                //if (distanceX < 0.05f)
                //{
                //    state = 2;
                //}



                break;
            //attack
            case 2:
                if (canAttack)
                {
                    animations.IsAttack = true;
                    animations.Attack();
                    AttackCD();
                    
                }
                
                
                break;
            //death
            case 3:
                animations.Death();
                //isMoving = false;
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Остановка
                break;
        }
    }


    private void Explosion()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        explosiveParticles.Play();

        if (hit != null)
        {
            playerStats = hit.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage * 3);

            }
        }
    }

    IEnumerator AttackCD()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }


    public void ChangeState()
    {
        state = 0;
        animations.IsAttack = false;
        animations.Attack();

    }

    public void DeathState()
    {
        int randomValue = Random.Range(0, 101);
        if (randomValue < rarity)
        {
            dropSystem.SpawnPrefab();

        }
        randomValue = Random.Range(0, 101);
        if (randomValue * 2 < rarity)
        {
            dropSystem.SpawnRandomCollectiblePrefab();
            if (randomValue < rarity)
            {
                dropSystem.SpawnRandomCollectiblePrefab();
                if (randomValue / 2 < rarity)
                {
                    dropSystem.SpawnRandomCollectiblePrefab();
                    if (randomValue / 4 < rarity)
                    {
                        dropSystem.SpawnRandomCollectiblePrefab();
                    }
                }
            }
        }
    }

}
