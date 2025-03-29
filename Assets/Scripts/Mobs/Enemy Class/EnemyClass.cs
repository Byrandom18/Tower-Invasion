using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyClass : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    private bool isChasing = false;
    [SerializeField] private float chaseDistance;
    private int state = 0;
    [SerializeField] private Transform EnemySprite;
    private float wallCheckDistance = 1f;
    public LayerMask wallLayer; // Слой, на котором находятся стены

    // Прыжок
    private bool isWallLeft = false;
    private bool isWallRight = false;
    [SerializeField] private float jumpForce = 8;
    private bool isGrounded = false;
    private bool isJumping = false;
    

    // ИИ
    private bool isUnderRoof = false;
    private bool canLaunchPF = true;
    private Vector3 directionPF;
    private int rndPF;
    private bool rndBoolPF;
    private bool isFarWallLeft = false;
    private bool isFarWallRight = false;
    private bool isFarDown = false;
    private bool isTopRightExist = false;
    private bool isTopLeftExist = false;
    private bool changedDirPF = false;

    // передвижение вне боя
    private bool isIdle = false;
    private bool isWalking = false;
    private float idleDuration = 3f;
    private float walkDuration = 4f;
    private float walkDirection;
    private bool isCliffLeft = false;
    private bool isCliffRight = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(playerTransform.position.y - transform.position.y);



        switch (state)
        {
            //idle
            case 0:
                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                {
                    state = 2;
                }

                if (!isIdle && !isChasing)
                {
                    StartCoroutine(IdleState());
                }

                break;

            //walk
            case 1:
                if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
                {
                    state = 2;
                }
                if (!isWalking && !isChasing)
                    StartCoroutine(WalkState());
                Walk();
                break;

            //chase
            case 2:
                isChasing = true;
                // проверка нахождения игрока над мобом под потолком и вызов стадии поиска пути
                CheckRoof();
                float distanceX = playerTransform.position.x - transform.position.x;
                float distanceY = playerTransform.position.y - transform.position.y;
                //Debug.Log(distanceY);
                CheckGround();
                if (((((distanceY > 2.5f) && isUnderRoof) && distanceY < 4f) || (distanceY < -2.5f)) && (Mathf.Abs(transform.position.x - playerTransform.position.x) < 0.2f) && isGrounded)
                {
                    state = 5;
                }
                CheckCliff();

                // прыжок
                CheckWall();
                
                
                if ((isWallRight && distanceX > 0) || (isWallLeft && distanceX < 0))
                {
                    
                    Jump();
                }

                if (distanceY > 3.5f && distanceX < 1f && distanceX > -1f)
                {
                    CheckFarWall();
                    state = 5;
                }


                // погоня
                if (transform.position.x > playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
                    
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
                else if (transform.position.x < playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(1, 1, 1);
                    
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }

                // с этой хуйней он ахуенно двигается, но ловит спайдер мен вайб при прыжке на стену
                //Vector2 direction = (playerTransform.position - transform.position).normalized;
                //rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);


                // tokyo drift vibe
                //Vector2 direction = (playerTransform.position - transform.position).normalized;
                //rb.AddForce(new Vector2(direction.x * speed * 10, 0));

                //// Ограничение максимальной скорости
                //if (Mathf.Abs(rb.velocity.x) > speed)
                //{
                //    rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * speed, rb.velocity.y);
                //}



                // завершение погони
                if (Vector2.Distance(transform.position, playerTransform.position) > chaseDistance * 2)
                {
                    state = 0;
                    isChasing = false;
                }
                break;

            //attack
            case 3:
                break;

            //death
            case 4:
                break;

            //path find
            case 5:
                PathFind();
                break;

        }
    }

    IEnumerator IdleState()
    {
        isIdle = true;
        yield return new WaitForSeconds(idleDuration);
        if (!isChasing)
        {
            state = 1; // Переключаем на движение
        }
        
        isIdle = false;
    }
    IEnumerator WalkState()
    {
        CheckWall();
        CheckCliff();
        isWalking = true;
        walkDirection = Random.Range(0, 2) == 0 ? -1f : 1f; // Случайное направление

        if (isWallLeft || isCliffLeft)
        {
            walkDirection = 1;
        }
        if (isWallRight || isCliffRight)
        {
            walkDirection = -1;
        }

        if (walkDirection < 0)
        {
            EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            EnemySprite.transform.localScale = new Vector3(1, 1, 1);
        }

        yield return new WaitForSeconds(walkDuration);
        if (!isChasing)
        {
            state = 0; // Переключаем на ожидание
        }
        isWalking = false;
    }
    void Walk()
    {
        CheckCliff();
        CheckWall();
        if (walkDirection == -1 && (isWallLeft || isCliffLeft))
        {
            state = 0;
            walkDirection = 0;
        }
        else if (walkDirection == 1 && (isWallRight || isCliffRight))
        {
            state = 0;
            walkDirection = 0;
        }
        rb.velocity = new Vector2(walkDirection * speed/2, rb.velocity.y);
    }


    private void CheckGround()
    {
        if (rb.velocity.y == 0)
        {
            isJumping = false;
        }
        Vector2 direction = -transform.up;
        float rayLength = 0.65f;
        // Проверка наличия земли
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, wallLayer);
        //Debug.DrawRay(rb.position, Vector2.down * rayLength, Color.red);
        if (hit.collider != null)
        {
            isGrounded = true;

        }
        else
        {
            isGrounded = false;
        }
        
    }

    private void CheckRoof()
    {
        Vector2 direction = transform.up;
        float rayLength = 1.5f;
        Vector2 size = new Vector2(2f, 1f);
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, size, 0f, direction, rayLength, wallLayer);

        if (hit.collider != null)
        {
            isUnderRoof = true;

        }
        else
        {
            isUnderRoof = false;
        }
        
    }

    private void Jump()
    {
        if (isGrounded && !isJumping)
        {
            isJumping = true;
            rb.velocity = new Vector2(0f, rb.velocity.y);
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            //animations.Jump();
        }
    }


    private void CheckWall()
    {
        Vector2 direction = transform.right;
        
        // Проверка наличия стены
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

        if (hit.collider != null)
        {
            isWallRight = true;
        }
        else
        {
            isWallRight = false;
        }

        direction = -transform.right;
        hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

        if (hit.collider != null)
        {
            isWallLeft = true;
        }
        else
        {
            isWallLeft = false;
        }


    }

    private void CheckFarWall()
    {
        Vector2 direction = transform.right;

        // Проверка наличия стены
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance * 4, wallLayer);

        if (hit.collider != null)
        {
            isFarWallRight = true;
        }
        else
        {
            isFarWallRight = false;
        }

        direction = -transform.right;
        hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance * 4, wallLayer);

        if (hit.collider != null)
        {
            isFarWallLeft = true;
        }
        else
        {
            isFarWallLeft = false;
        }
    }

    private void PathFind()
    {
        float distance = playerTransform.position.y - transform.position.y;
        // up
        if (distance > 2.5f && distance < 3.5f && !isFarDown)
        {
            if (canLaunchPF)
            {
                canLaunchPF = false;
                rndPF = Random.Range(0, 2);
                rndBoolPF = rndPF == 1;
            }
            
            CheckWall();
            if (isWallRight)
            {
                rndBoolPF = false;
            }
            else if (isWallLeft)
            {
                rndBoolPF = true;
            }

            if (rndBoolPF)
            {
                directionPF = Vector3.right;
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                directionPF = Vector3.left;
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            CheckRoof();
            
            

            transform.position += directionPF * speed * Time.deltaTime;
            

            if (!isUnderRoof)
            {
                
                canLaunchPF = true;
                Jump();
                state = 2;
            }
            
        }

        // down
        if (distance < -1f && !isFarDown)
        {
            if (canLaunchPF)
            {
                canLaunchPF = false;
                rndPF = Random.Range(0, 2);
                rndBoolPF = rndPF == 1;
            }

            CheckWall();
            if (isWallRight)
            {
                rndBoolPF = false;
            }
            else if (isWallLeft)
            {
                rndBoolPF = true;
            }

            if (rndBoolPF)
            {
                directionPF = Vector3.right;
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                directionPF = Vector3.left;
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            
            
            
            transform.position += directionPF * speed * Time.deltaTime;
            

            
            if ((distance) > -1f)
            {
                
                canLaunchPF = true;
                state = 2;
            }


        }

        if (distance > 3.5f || isFarDown)
        {
            CheckTopEmpty();
            isFarDown = true;
            CheckFarWall();
            CheckWall();
            CheckGround();
            CheckRoof();
            if (isFarWallLeft && isGrounded && !changedDirPF)
            {
                directionPF = Vector3.left;
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            if (isFarWallRight && isGrounded && !changedDirPF)
            {
                directionPF = Vector3.right;
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
            }
            
            if ((isWallLeft && !isTopLeftExist) || (isWallRight && !isTopRightExist))
            {
                Jump();
            }
            else if (isWallLeft && isTopLeftExist && !changedDirPF && isGrounded)
            {
                directionPF = Vector3.right;
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
                changedDirPF = true;
            }
            else if (isWallLeft && isTopLeftExist && !changedDirPF && isGrounded)
            {
                directionPF = Vector3.left;
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
                changedDirPF = true;
            }

            transform.position += directionPF * speed * Time.deltaTime;
            
            if (changedDirPF && !isUnderRoof && ((directionPF.x == -1 && isTopLeftExist) || (directionPF.x == 1 && isTopRightExist)))
            {
                Jump();
            }

            if (distance < 3.5f && isGrounded)
            {
                canLaunchPF = true;
                isFarWallLeft = false;
                isFarWallRight = false;
                state = 2;
                isFarDown = false;
                changedDirPF = false;
            }
            else
            {
                CheckFarWall();
            }
        }



        if (((distance) > -1.5f) && ((distance) < 1.5f) && isGrounded)
        {
            isFarDown = false;
            canLaunchPF = true;
            state = 2;
            isFarWallLeft = false;
            isFarWallRight = false;
            changedDirPF = false;
        }

    }


    private void CheckCliff()
    {
        float distanceY = playerTransform.position.y - transform.position.y;
        float distanceX = playerTransform.position.x - transform.position.x;
        if (distanceX < 0)
        {
            distanceX = -distanceX;
        }
        Vector2 originalDirection = Vector2.up; // Исходное направление (1, 0)
        float angle = 150f; // Угол поворота

        // Поворачиваем вектор на 45 градусов
        Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * originalDirection;
        Vector2 rotatedDirection2 = Quaternion.Euler(0, 0, -angle) * originalDirection;
        // Используем в Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rotatedDirection, 1.5f, wallLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, rotatedDirection2, 1.5f, wallLayer);

        //Debug.DrawRay(transform.position, rotatedDirection * 1.5f, Color.red);
        //Debug.DrawRay(transform.position, rotatedDirection2 * 1.5f, Color.red);
        //Debug.Log(distanceY);
        if (isChasing && (distanceY > -0.2f) && ((hit.collider == null) || (hit2.collider == null)) && distanceX > 1f)
        {
            Jump();
        }
        if (hit.collider == null)
        {
            isCliffLeft = true;
        }
        else
        {
            isCliffLeft = false;
        }
        if (hit2.collider == null)
        {
            isCliffRight = true;
        }
        else
        {
            isCliffRight = false;
        }
    }

    private void CheckTopEmpty()
    {
        Vector2 originalDirection = Vector2.up; // Исходное направление (1, 0)
        float angle = 35f; // Угол поворота
        float rayDistance = 2.5f;
        // Поворачиваем вектор на 45 градусов
        Vector2 rotatedDirection = Quaternion.Euler(0, 0, angle) * originalDirection;
        Vector2 rotatedDirection2 = Quaternion.Euler(0, 0, -angle) * originalDirection;

        // Используем в Raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rotatedDirection, rayDistance, wallLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, rotatedDirection2, rayDistance, wallLayer);

        //Debug.DrawRay(transform.position, rotatedDirection * rayDistance, Color.red);
        //Debug.DrawRay(transform.position, rotatedDirection2 * rayDistance, Color.red);

        if (hit.collider != null)
        {
            isTopLeftExist = true;
        }
        else
        {
            isTopLeftExist = false;
        }
        if (hit2.collider != null)
        {
            isTopRightExist = true;
        }
        else
        {
            isTopRightExist = false;
        }
        
    }


}
