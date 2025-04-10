using UnityEngine;

public class RogueAnimations : MonoBehaviour, IEnemyAnimations
{
    private Animator animator;

    public bool IsMoving {get; set; }
    public bool IsFlying {get; set; }
    public bool IsAttack {get; set; }
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
