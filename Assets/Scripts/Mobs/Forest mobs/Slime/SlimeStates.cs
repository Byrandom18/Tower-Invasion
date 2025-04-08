using UnityEngine;

public class SlimeStates : MonoBehaviour
{
    

    private EnemyClass enemy;
    private SlimeAnimations animations;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animations = GetComponentInChildren<SlimeAnimations>();
        enemy = GetComponentInChildren<EnemyClass>();
    }

    

    public void Attack()
    {
        
    }

    public void ChangeState()
    {
        enemy.state = 2;
        animations.IsAttack = false;
        animations.Attack();
        
    }


    public void Death()
    {

    }
}
