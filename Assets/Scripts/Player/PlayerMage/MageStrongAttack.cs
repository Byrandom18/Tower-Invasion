using UnityEngine;

public class MageStrongAttack : MonoBehaviour
{
    public GameObject strongAttackPrefab; // Префаб сильной атаки
    public float strongAttackCooldown = 5f; // Перезарядка сильной атаки
    private float lastStrongAttackTime;

    void Update()
    {
        // Сильная атака по нажатию клавиши (например, E)
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastStrongAttackTime + strongAttackCooldown)
        {
            StrongAttack();
        }
    }

    void StrongAttack()
    {
        lastStrongAttackTime = Time.time;

        // Получаем позицию мыши
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;

        // Создаем эффект сильной атаки
        GameObject strongAttack = Instantiate(strongAttackPrefab, transform.position, Quaternion.identity);
        // Можно добавить дополнительную логику (например, радиус поражения)
        Debug.Log("Сильная атака выполнена!");
    }
}
