using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float speed = 2;
    private Transform player;
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    private int state = 0;
    [SerializeField] private Transform EnemySprite;
    private float wallCheckDistance = 1f;
    public LayerMask wallLayer; // Слой, на котором находятся стены

    // Прыжок
    private bool isWallNearby = false;
    [SerializeField] private float jumpForce = 8;
    private bool isGrounded = false;
    

    // ИИ
    private bool isUnderRoof = false;
    private bool canLaunchPF = true;
    private Vector3 directionPF;
    private bool canChangeDirectionPF = true;
    private int rndPF;
    bool rndBoolPF;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
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
                float distance = playerTransform.position.y - transform.position.y;
                CheckGround();
                if (((((distance > 2.5f) && isUnderRoof) && distance < 4f) || (distance < -2.5f)) && (Mathf.Abs(transform.position.x - playerTransform.position.x) < 0.2f))
                {
                    state = 5;
                }


                // прыжок
                CheckWall();
                
                
                if (isWallNearby)
                {
                    
                    Jump();
                }




                // погоня
                if (transform.position.x > playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
                    //EnemySprite.flipX = true;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
                else if (transform.position.x < playerTransform.position.x)
                {
                    EnemySprite.transform.localScale = new Vector3(1, 1, 1);
                    //EnemySprite.flipX = false;
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }

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
        Vector2 direction = -transform.up;
        float rayLength = 0.7f;
        // Проверка наличия земли
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, wallLayer);

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
        if (isGrounded)
        {
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
            isWallNearby = true;
        }
        else
        {
            direction = -transform.right;
            hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);
            
            if (hit.collider != null)
            {
                isWallNearby = true;
            }
            else
            {
                isWallNearby = false;
            }
        }


    }

    private void CheckLeftWall()
    {

    }
    private void CheckRightWall()
    {

    }

    private void PathFind()
    {
        float distance = playerTransform.position.y - transform.position.y;
        // up
        if (distance > 2.5)
        {
            if (canLaunchPF)
            {
                canLaunchPF = false;
                canChangeDirectionPF = true;
                rndPF = Random.Range(0, 2);
                rndBoolPF = rndPF == 1;
            }
            
            CheckWall();
            if (canChangeDirectionPF && isWallNearby)
            {
                canChangeDirectionPF = false;
                rndBoolPF = !rndBoolPF;
            }
            if (rndBoolPF)
            {
                directionPF = Vector3.right;
            }
            else
            {
                directionPF = Vector3.left;
            }
            CheckRoof();
            
            if (rndBoolPF)
            {
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            transform.position += directionPF * speed * Time.deltaTime;
            

            if (!isUnderRoof)
            {
                
                canLaunchPF = true;
                Jump();
                state = 2;
            }
            
        }

        // down
        if (distance < -1f)
        {
            if (canLaunchPF)
            {
                canLaunchPF = false;
                canChangeDirectionPF = true;
                rndPF = Random.Range(0, 2);
                rndBoolPF = rndPF == 1;
            }
            

            if (rndBoolPF)
            {
                directionPF = Vector3.right;
            }
            else
            {
                directionPF = Vector3.left;
            }
            
            CheckWall();
            if (rndBoolPF)
            {
                EnemySprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                EnemySprite.transform.localScale = new Vector3(-1, 1, 1);
            }
            transform.position += directionPF * speed * Time.deltaTime;
            if (canChangeDirectionPF && isWallNearby)
            {
                canChangeDirectionPF = false;
                rndBoolPF = !rndBoolPF;
            }

            
            if ((distance) > -1f)
            {
                
                canLaunchPF = true;
                state = 2;
            }


        }
        if (((distance) > -1.5f) && ((distance) < 1.5f))
        {
            
            canLaunchPF = true;
            state = 2;
        }

    }
}
