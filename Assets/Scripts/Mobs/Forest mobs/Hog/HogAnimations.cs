using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HogAnimations : MonoBehaviour
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

    public void Death()
    {
        animator.SetTrigger("Death");

        StartCoroutine(DeathWait());
    }
    IEnumerator DeathWait()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    
}
