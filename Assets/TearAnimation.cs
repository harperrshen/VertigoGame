using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TearAnimation : MonoBehaviour
{
    public Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(animator!=null){animator.gameObject.SetActive(true);}
        }
    }
    
    private void Update()
    {
        if (animator!=null && animator.gameObject.activeSelf && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.98f && animator.GetCurrentAnimatorStateInfo(0).IsName("Dali's Tear"))
        {   
            Destroy(animator.gameObject);
        }
    }
}
