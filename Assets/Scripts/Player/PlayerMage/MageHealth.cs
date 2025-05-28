using UnityEngine;

public class MageHealth : MonoBehaviour
{
    public float maxHealth = 100f; // Максимальное здоровье
    public float currentHealth; // Текущее здоровье
    public float potionHealAmount = 25f; // Количество лечения от зелья
    public int potionCount = 3; // Количество зелий

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Использование зелья по нажатию клавиши (например, H)
        if (Input.GetKeyDown(KeyCode.H) && potionCount > 0 && currentHealth < maxHealth)
        {
            UsePotion();
        }
    }

    void UsePotion()
    {
        potionCount--;
        currentHealth = Mathf.Min(currentHealth + potionHealAmount, maxHealth);
        Debug.Log($"Зелье использовано! Текущее здоровье: {currentHealth}, Зелий осталось: {potionCount}");
    }

    // Метод для получения урона (для тестов или взаимодействия с врагами)
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Маг умер!");
        // Здесь можно добавить логику смерти (например, перезапуск сцены или анимацию)
        gameObject.SetActive(false); // Временно отключаем персонажа
    }
}