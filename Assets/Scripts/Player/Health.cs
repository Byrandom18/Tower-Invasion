using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f; // ������������ ��������
    private float currentHealth;   // ������� ��������
    private bool isDead = false;   // ���� ������

    // ���������� ��� ������������
    public float knockbackForce = 5f; // ���� ������������
    public float knockbackDuration = 0.2f; // ������������ ������������
    private Rigidbody2D rb; // ������ �� Rigidbody2D
    private bool isKnockedBack = false; // ���� ��������� ������������
    private float knockbackTimer; // ������ ������������

    private Animator animator; // ��� ��������

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>(); // �������� Rigidbody2D

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D �� ������ �� ������� ������! ������������ �� ����� ��������.");
        }
    }

    void Update()
    {
        // ��������� ������������
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
                rb.linearVelocity = Vector2.zero; // ������������� �������� ����� ������������
            }
        }
    }

    // ����� ��� ��������� ����� � �������������
    public void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (isDead) return; // �� �������� ����, ���� ������

        // ��������� ����
        currentHealth -= damage;
        Debug.Log($"����� ������� {damage} �����. ������� ��������: {currentHealth}");

        // ������������
        if (rb != null && !isKnockedBack)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)attackerPosition).normalized;
            rb.linearVelocity = knockbackDirection * knockbackForce; // ��������� ���� ������������
            isKnockedBack = true;
            knockbackTimer = knockbackDuration;

            // �����������: ������ �������� ��������� �����
            animator.SetTrigger("Hurt");
        }

        // �������� ������
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("����� ����!");
        animator.SetTrigger("Death");

        // ��������� ����������
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<PlayerStrongAttack>().enabled = false;
        GetComponent<PlayerBlock>().enabled = false;

        // ������������� ��������
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // ����� ����� �������� ������ ����� ����
        // Invoke("RestartLevel", 2f);
    }

    // ����� ��� ��������� �������� �������� �������� (��� UI)
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }

    // ����� ��� ��������, ��� �� �����
    public bool IsAlive()
    {
        return !isDead;
    }
}