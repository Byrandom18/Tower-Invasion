using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    private bool isChasing;
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
    //private bool cliffNearby = false;
    private bool isFarWallLeft = false;
    private bool isFarWallRight = false;
    private bool isFarDown = false;


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
                break;

            //walk
            case 1:
                break;

            //chase
            case 2:
                // проверка нахождения игрока над мобом под потолком и вызов стадии поиска пути
                CheckRoof();
                float distanceX = playerTransform.position.x - transform.position.x;
                float distanceY = playerTransform.position.y - transform.position.y;
                Debug.Log(distanceY);
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
            isFarDown = true;
            CheckFarWall();
            if (isFarWallLeft && isGrounded)
            {
                directionPF = Vector3.left;
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            if (isFarWallRight && isGrounded)
            {
                directionPF = Vector3.right;
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
            }

            transform.position += directionPF * speed * Time.deltaTime;
            CheckWall();
            CheckGround();
            if (isWallLeft || isWallRight)
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
            }
            else
            {
                CheckFarWall();
            }
        }



        if (((distance) > -1.5f) && ((distance) < 1.5f) && isGrounded)
        {
            
            canLaunchPF = true;
            state = 2;
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
        if ((distanceY > -0.2f) && ((hit.collider == null) || (hit2.collider == null)) && distanceX > 1f)
        {
            Jump();
        }
        //if ((hit.collider == null) || (hit2.collider == null))
        //{
        //    cliffNearby = true;
        //}
        //else
        //{
        //    cliffNearby = false;
        //}
    }


}
