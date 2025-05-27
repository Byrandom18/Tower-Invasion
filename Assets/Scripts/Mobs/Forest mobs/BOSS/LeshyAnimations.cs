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
        animator.SetTrigger("Slam");
    }

    public void Bomb()
    {
        animator.SetTrigger("Bomb");
    }

    public void Spikes()
    {
        animator.SetTrigger("Spikes");
    }

    public void Spam()
    {
        animator.SetTrigger("Spam");
    }

    public void Punch()
    {
        //animator.SetTrigger("Punch");
        animator.SetInteger("Attack", 1);
    }

    public void Summon()
    {
        animator.SetTrigger("Summon");
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
