using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator animator;

    public bool IsMoving { private get; set; }
    public bool IsFlying { private get; set; }
    
    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        animator.SetBool("IsMoving", IsMoving);
        animator.SetBool("IsFlying", IsFlying);
    }

    public void Jump()
    {
        animator.SetTrigger("Jump");
    }
}
