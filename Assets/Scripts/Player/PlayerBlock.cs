using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
    private bool isBlocking = false;
    public float blockDamageReduction = 0.5f; // Уменьшение урона на 50% при блоке
    private Animator animator;
    private PlayerHealth playerHealth; // Ссылка на здоровье

    void Start()
    {
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();

        // Проверка на наличие компонентов
        if (animator == null)
        {
            Debug.LogError("Animator не найден на объекте " + gameObject.name);
        }
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth не найден на объекте " + gameObject.name);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            StartBlocking();
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
            StopBlocking();
        }
    }

    void StartBlocking()
    {
        isBlocking = true;
        animator.SetBool("IsBlocking", true);
        Debug.Log("Блок активирован");
    }

    void StopBlocking()
    {
        isBlocking = false;
        animator.SetBool("IsBlocking", false);
        Debug.Log("Блок деактивирован");
    }

    // Метод для обработки урона с учетом блока
    public void TakeDamage(float damage, Vector2 attackerPosition)
    {
        if (playerHealth == null || !playerHealth.IsAlive())
        {
            Debug.LogWarning("Игрок мертв или PlayerHealth отсутствует");
            return;
        }

        // Уменьшаем урон, если блок активен
        float finalDamage = isBlocking ? damage * blockDamageReduction : damage;
        Debug.Log($"Получен урон: {damage}, Блок: {isBlocking}, Итоговый урон: {finalDamage}");

        // Передаем итоговый урон в PlayerHealth
        playerHealth.TakeDamage(finalDamage, attackerPosition);
    }

    public bool IsBlocking()
    {
        return isBlocking;
    }
}