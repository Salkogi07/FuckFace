using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill3 : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(SkillStart());
    }

    private IEnumerator SkillStart()
    {
        while (true)
        {
            animator.Play("Skill1");
            yield return new WaitForSeconds(RandomWait());
            animator.Play("Skill2");
            yield return new WaitForSeconds(RandomWait());
            animator.Play("Skill3");
            yield return new WaitForSeconds(RandomWait());
            animator.Play("Skill4");
            yield return new WaitForSeconds(RandomWait());
            animator.Play("Skill5");
            yield return new WaitForSeconds(RandomWait());
        }
    }

    private float RandomWait() => Random.Range(1, 3);
}
