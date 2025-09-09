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
        stats = GetComponentInChildren<PlayerStats>();
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

        if (PlayerManager.instance.GetMasterPlayer() != this.gameObject)
            return;

        #region ��ų ���
        // �Է� ó�� (����)
        if (Input.GetKeyDown(KeyCode.A))
        {
            UseBuffSkill(0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            UseBuffSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            UseBuffSkill(2);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            UseBuffSkill(3);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            UseBuffSkill(4);
        }
        #endregion
    }

    public void UseDamageSkill(int index)
    {
        if (index < 0 || index > damageSkills.Count) return;

        DamageSkill skill = damageSkills[index];
        if (skill.CanUse(stats))
        {
            skill.Activate(this);
        }
    }

    public void UseBuffSkill(int index)
    {
        if (index < 0 || index > buffSkills.Count) return;

        BuffSkill skill = buffSkills[index];
        if (skill.CanUse(stats))
        {
            skill.Activate(this);
        }
    }

    public PlayerStats GetPlayerStats() => stats;
    public Animator GetAnimator() => animator;
}