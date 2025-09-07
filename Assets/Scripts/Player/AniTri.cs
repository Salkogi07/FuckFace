using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniTri : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StopAnim()
    {
        animator.SetTrigger("Idle");
    }
}
