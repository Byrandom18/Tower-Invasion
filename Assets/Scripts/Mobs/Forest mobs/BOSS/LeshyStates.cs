using System.Collections;
using System.Collections.Generic;
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        
        animations = GetComponentInChildren<LeshyAnimations>();
        
        
    }


    private void Update()
    {
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






}
