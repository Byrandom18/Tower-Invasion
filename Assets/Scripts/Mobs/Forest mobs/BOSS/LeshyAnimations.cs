using UnityEngine;

public class LeshyAnimations : MonoBehaviour
{
    private Animator animator;

    public bool IsMoving { get; set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool("IsMoving", IsMoving);
        
    }

    public void Slam()
    {
        animator.SetInteger("Attack", 5);
    }

    public void Bomb()
    {
        animator.SetInteger("Attack", 6);
    }

    public void Spikes()
    {
        animator.SetInteger("Attack", 4);
    }

    public void Spam()
    {
        animator.SetInteger("Attack", 2);
    }

    public void Punch()
    {
        //animator.SetTrigger("Punch");
        animator.SetInteger("Attack", 1);
    }

    public void Summon()
    {
        animator.SetInteger("Attack", 3);
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }


    public void ResetAttack()
    {
        animator.SetInteger("Attack", 0);
    }
}
