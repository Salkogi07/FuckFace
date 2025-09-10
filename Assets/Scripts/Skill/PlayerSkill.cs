using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    public GameObject main;
    private PlayerStats stats;
    private Animator animator;

    public List<DamageSkill> damageSkills = new List<DamageSkill>();
    public List<BuffSkill> buffSkills = new List<BuffSkill>();

    public Text[] skillNameText;
    public Image[] skillCool;

    public Text[] CmskillNameText;
    public Image[] CmskillCool;

    private void Awake()
    {
        stats = main.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 모든 데미지 스킬의 쿨타임을 업데이트합니다.
        foreach (var skill in damageSkills)
        {
            skill.Tick();
        }

        // 모든 버프 스킬의 쿨타임을 업데이트합니다.
        foreach (var skill in buffSkills)
        {
            skill.Tick();
        }

        for(int i = 0; i < damageSkills.Count; i++)
        {
            DamageSkill skill = damageSkills[i];
            int lv = skill.lv;
            float fill = skill.currentCoolTime[lv] / skill.coolTime[lv];

            skillNameText[i].text = skill.name;

            if(skill.currentCoolTime[lv] > 0)
                skillCool[i].fillAmount = fill;
            else
                skillCool[i].fillAmount = 0;

        }

        for (int i = 0; i < buffSkills.Count; i++)
        {
            BuffSkill skill = buffSkills[i];
            int lv = skill.lv;
            float fill = skill.currentCoolTime[lv] / skill.coolTime[lv];

            CmskillNameText[i].text = skill.name;

            if (skill.currentCoolTime[lv] > 0)
                CmskillCool[i].fillAmount = fill;
            else
                CmskillCool[i].fillAmount = 0;
        }

        if (PlayerManager.instance.GetMasterPlayer() != main)
            return;

        #region 스킬 사용
        // 입력 처리 (예시)
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