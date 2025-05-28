using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;

public class LeshyStates : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    public int state = 0;
    [SerializeField] private Transform EnemySprite;
    private bool isMoving = false;

    //public MonoBehaviour animationsScript;
    private LeshyAnimations animations;

    //[SerializeField] private float attackRate = 0.5f;
    //private float nextAttackTime = 0f;
    //[SerializeField] private float attackRange = 1f;

    private bool isIdle = false;
    
    private float idleDuration = 1f;
    private bool isPreparating = false;
    [SerializeField] private float preparationDuration = 3f;
    private Vector2 attackDirection;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private PlayerStats playerStats; // Перетащите игрока в инспекторе
    [SerializeField] private EnemyDamage enemyDamage; // Перетащите компонент с уроном
    private float damage;
    public GameObject projectilePrefab;
    public float projectileSpeed = 8f;
    public float projectileLifetime = 5f;
    [SerializeField] private DropSystem dropSystem; // Перетащите в инспекторе
    [SerializeField] private int rarity = 0;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerStats = player.GetComponent<PlayerStats>();
        enemyDamage = GetComponent<EnemyDamage>();
        animations = GetComponentInChildren<LeshyAnimations>();
        dropSystem = GetComponent<DropSystem>();
        damage = enemyDamage.Damage;
    }


    private void Update()
    {
        if (enemyDamage.health <= 0)
        {
            animations.Death();
        }
        animations.IsMoving = isMoving;
        switch (state)
        {
            
            //idle
            case 0:
                if (!isIdle)
                {
                    StartCoroutine(IdleState());
                }
                break;
            //chase
            case 1:
                float distanceX = playerTransform.position.x - transform.position.x;

                if (transform.position.x > playerTransform.position.x && distanceX < -1f)
                {
                    EnemySprite.transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта
                    isMoving = true;
                    rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y); // Движение влево
                }
                else if (transform.position.x < playerTransform.position.x && distanceX > 1f)
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

                if (!isPreparating)
                {
                    StartCoroutine(Preparation());
                }
                break;
            //ground slam
            case 2:
                animations.Slam();
                state = 0;
                break;
            //toxic bomb
            case 3:
                animations.Bomb();
                state = 0;
                break;
            //spikes
            case 4:
                animations.Spikes();
                state = 0;
                break;
            //praise the sun
            case 5:
                animations.Spam();
                state = 0;
                break;
            //fast punch
            case 6:
                animations.Punch();
                state = 0;
                break;
            //mushroom summon
            case 7:
                animations.Summon();
                state = 0;
                break;
            //death
            case 8:
                animations.Death();
                break;
        }
    }

    IEnumerator IdleState()
    {
        isMoving = false;
        isIdle = true;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        yield return new WaitForSeconds(idleDuration);
        

        isIdle = false;
        state = 1;
    }

    IEnumerator Preparation()
    {
        
        isPreparating = true;
        yield return new WaitForSeconds(preparationDuration);
        isMoving = false;
        int randomValue = Random.Range(2, 8);
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        state = randomValue;
        isPreparating = false;

        
        if (transform.position.x > playerTransform.position.x)
        {
            EnemySprite.transform.localScale = new Vector3(-1, 1, 1); // Разворот спрайта
            
        }
        else if (transform.position.x < playerTransform.position.x)
        {
            EnemySprite.transform.localScale = new Vector3(1, 1, 1); // Разворот спрайта
            
        }
    }

    private void AtkFast()
    {
        Vector2 attackStartPoint = transform.position + Vector3.down * 0.5f;
        attackDirection = transform.right * Mathf.Sign(transform.localScale.x);

        //Debug.DrawRay(attackStartPoint, attackDirection * attackRange, Color.red, 1f); // Визуализация в Game View

        RaycastHit2D hit = Physics2D.Raycast(
            attackStartPoint,
            attackDirection,
            attackRange,
            playerLayer
        );

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player")) // Дополнительная проверка
            {
                playerStats.TakeDamage(damage);
                
            }
        }
        
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
