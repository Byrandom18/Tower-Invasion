using UnityEngine;

public class MageAttack : MonoBehaviour
{
    public GameObject projectilePrefab; // ������ ����������� ����
    public float projectileSpeed = 10f; // �������� �������
    public float attackCooldown = 0.5f; // ����������� �����
    private float lastAttackTime;

    void Update()
    {
        // ����� �� ������� ����� ������ ����
        if (Input.GetMouseButtonDown(0) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        // �������� ������� ���� � ������� �����������
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        // ������� ������
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * projectileSpeed;
    }
}
