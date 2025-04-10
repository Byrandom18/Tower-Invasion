using UnityEngine;

public interface IEnemyAnimations
{
    // Свойства для управления анимациями
    bool IsMoving { get; set; }
    bool IsFlying { get; set; }
    bool IsAttack { get; set; }

    // Методы для триггеров анимаций
    void Attack();
    void Death();
}