using System.Collections.Generic;
using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    private PlayerStats stats;
    private Animator animator;

    public List<DamageSkill> damageSkills = new List<DamageSkill>();
    public List<BuffSkill> buffSkills = new List<BuffSkill>();

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ��� ������ ��ų�� ��Ÿ���� ������Ʈ�մϴ�.
        foreach (var skill in damageSkills)
        {
            skill.Tick();
        }

        // ��� ���� ��ų�� ��Ÿ���� ������Ʈ�մϴ�.
        foreach (var skill in buffSkills)
        {
            skill.Tick();
        }

        // �Է� ó�� (����)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseDamageSkill(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UseDamageSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseBuffSkill(0);
        }
    }

    public void UseDamageSkill(int index)
    {
        if (index < 0 || index >= damageSkills.Count) return;

        DamageSkill skill = damageSkills[index];
        if (skill.CanUse(stats))
        {
            skill.Activate(this);
        }
    }

    public void UseBuffSkill(int index)
    {
        if (index < 0 || index >= buffSkills.Count) return;

        BuffSkill skill = buffSkills[index];
        if (skill.CanUse(stats))
        {
            skill.Activate(this);
        }
    }

    public PlayerStats GetPlayerStats() => stats;
    public Animator GetAnimator() => animator;
}