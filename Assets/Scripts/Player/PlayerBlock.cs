using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private bool isBlocking = false;
    public float blockDamageReduction = 0.5f; // Уменьшение урона на 50%
    private Animator animator;
    private PlayerHealth playerHealth; // Ссылка на здоровье

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartBlocking();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            StopBlocking();
        }
    }

    void StartBlocking()
    {
        isBlocking = true;
        animator.SetBool("IsBlocking", true);
    }

    void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("IsBlocking", false);
    }

    // Обновленный метод с учетом позиции атакующего
    public void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (playerHealth == null || !playerHealth.IsAlive()) return;

        // Уменьшаем урон при блоке
        float finalDamage = isBlocking ? damage * blockDamageReduction : damage;
        playerHealth.TakeDamage(finalDamage, attackerPosition); // Передаем урон и позицию
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}