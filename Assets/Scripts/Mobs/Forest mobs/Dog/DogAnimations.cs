using UnityEngine;

public class DogAnimations : MonoBehaviour, IEnemyAnimations
{

    private Animator animator;

    public bool IsMoving { get; set; }
    public bool IsFlying { get; set; }
    public bool IsAttack { get; set; }
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        animator.SetBool("IsMoving", IsMoving);
        animator.SetBool("IsFlying", IsFlying);
    }

    public void Attack()
    {
        animator.SetBool("IsAttack", IsAttack);
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
}

