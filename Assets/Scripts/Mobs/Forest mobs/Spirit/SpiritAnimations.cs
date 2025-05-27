using UnityEngine;

public class SpiritAnimations : MonoBehaviour
{
    private Animator animator;
    public bool IsAttack { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        animator = GetComponent<Animator>();
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
