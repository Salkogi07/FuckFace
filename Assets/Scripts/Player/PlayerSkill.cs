using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill
{
    public string name;
    public int lv;
    public float[] coolTIme, currentCoolTIme, needMP, power;
    public int[] count;
    public string animationName;
    public DamageTrigger damageTriggers;
}
public class PlayerSkill : MonoBehaviour
{
    private PlayerStats stats;

    public Animator animator;

    public Skill[] skillJobData = new Skill[4];
    public Skill[] skillCommonData = new Skill[5];

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        for (int i = 0; i < skillJobData.Length; i++)
        {
            for (int j = 0; j < skillJobData[i].currentCoolTIme.Length; j++)
            {
                if (skillJobData[i].currentCoolTIme[j] < skillJobData[i].coolTIme[j])
                {
                    skillJobData[i].currentCoolTIme[j] += Time.deltaTime;
                }
            }

            for(int k = 0; k < skillJobData.Length; k++)
            {
                skillJobData[i].damageTriggers.damage = skillJobData[i].power[k];
                skillJobData[i].damageTriggers.count = skillJobData[i].count[k];
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            UseSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UseSkill(2);
        }
    }

    public void UseSkill(int index)
    {
        int lv = skillJobData[index].lv;
        Skill skill = skillJobData[index];

        if (lv > 0)
        {
            if (skill.needMP[lv] <= stats.currentMP)
            {
                if (skill.coolTIme[lv] <= skill.currentCoolTIme[lv])
                {
                    UIManager.Instance.ShowMsg(stats.playerType + " : " + skill.name);
                    skill.currentCoolTIme[lv] = 0;
                    stats.currentMP -= skill.needMP[lv];
                    skill.damageTriggers.i = 0;
                    animator.Play(skill.animationName);
                }
            }
        }
    }
}
