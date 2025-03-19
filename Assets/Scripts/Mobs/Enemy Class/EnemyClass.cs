using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{

    [SerializeField] private int health = 100;

    
    [SerializeField] private float speed = 2;

    private Transform player;
    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    private int state = 0;
    [SerializeField] private SpriteRenderer EnemySprite;
    private float wallCheckDistance = 1f;
    public LayerMask wallLayer; // Слой, на котором находятся стены

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

      

        CheckWall();




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
                
                // погоня
                if (transform.position.x > playerTransform.position.x)
                {
                    EnemySprite.flipX = false;
                    transform.position += Vector3.left * speed * Time.deltaTime;
                }
                if (transform.position.x < playerTransform.position.x)
                {
                    EnemySprite.flipX = true;
                    transform.position += Vector3.right * speed * Time.deltaTime;
                }

                // завершение погони
                if (Vector2.Distance(transform.position, playerTransform.position) > chaseDistance)
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
            

        }
    }

    private void CheckWall()
    {
        Vector2 direction = transform.right;

        // Проверка наличия стены
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, wallLayer);

        if (hit.collider != null)
        {
            Debug.Log("Стена рядом!");
        }
        else
        {
            Debug.Log("Стены нет рядом.");
        }

    }
   
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

}

