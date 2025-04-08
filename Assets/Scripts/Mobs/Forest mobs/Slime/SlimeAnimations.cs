using UnityEngine;

public class SlimeAnimations : MonoBehaviour
{
    private Animator animator;

    public bool IsMoving { private get; set; }
    public bool IsFlying { private get; set; }
    public bool IsAttack { private get; set; }
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
        animator.SetBool("Attack", IsAttack);
    }

    public void Death()
    {
        animator.SetTrigger("Death");
    }
}
