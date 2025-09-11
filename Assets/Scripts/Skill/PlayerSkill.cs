using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillUI
{
    public Text[] skillNameText;
    public Image[] skillCool;
    public Text[] upSkillName;
    public Text[] upSkillLv;
    public Button[] upbuttons;
}

[Serializable]
public class CMSkillUI
{
    public Text[] skillNameText;
    public Image[] skillCool;
    public Text[] upSkillName;
    public Text[] upSkillLv;
    public Button[] upbuttons;
}


public class PlayerSkill : MonoBehaviour
{
    public GameObject main;
    public Text skillPoint;
    public GameObject skillPanel;
    private PlayerStats stats;
    private Animator animator;

    public List<DamageSkill> damageSkills = new List<DamageSkill>();
    public List<BuffSkill> buffSkills = new List<BuffSkill>();

    public SkillUI skillui;
    public CMSkillUI Cmskillui;
    

    private void Awake()
    {
        stats = main.GetComponent<PlayerStats>();
        animator = GetComponent<Animator>();

        for (int i = 0; i < skillui.upbuttons.Length; i++)
        {
            int index = i;
            skillui.upbuttons[index].onClick.AddListener(() => UpgradeDamageSkill(index));
        }

        for (int i = 0; i < Cmskillui.upbuttons.Length; i++)
        {
            int index = i;
            Cmskillui.upbuttons[index].onClick.AddListener(() => UpgradeBuffSkill(index));
        }
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            skillPanel.SetActive(!skillPanel.activeSelf);
            Time.timeScale = skillPanel.activeSelf ? 0f : 1f;
        }

        if (PlayerManager.instance.GetMasterPlayer() != main)
            return;

        skillPoint.text = "남은 스킬 포인트 : " + stats.skillPoint;

        UpdateSkillUI();
        UpdateCmSkillUI();

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

    private void UpdateCmSkillUI()
    {
        for (int i = 0; i < buffSkills.Count; i++)
        {
            BuffSkill skill = buffSkills[i];
            int lv = skill.lv;
            float fill = skill.currentCoolTime[lv] / skill.coolTime[lv];

            Cmskillui.skillNameText[i].text = skill.name;
            Cmskillui.upSkillName[i].text = skill.name;
            Cmskillui.upSkillLv[i].text = "Lv." + skill.lv.ToString();

            if (skill.currentCoolTime[lv] > 0)
                Cmskillui.skillCool[i].fillAmount = fill;
            else
                Cmskillui.skillCool[i].fillAmount = 0;
        }
    }

    private void UpdateSkillUI()
    {
        for (int i = 0; i < damageSkills.Count; i++)
        {
            DamageSkill skill = damageSkills[i];
            int lv = skill.lv;
            float fill = skill.currentCoolTime[lv] / skill.coolTime[lv];

            skillui.skillNameText[i].text = skill.name;
            skillui.upSkillName[i].text = skill.name;
            skillui.upSkillLv[i].text = "Lv." + skill.lv.ToString();

            if (skill.currentCoolTime[lv] > 0)
                skillui.skillCool[i].fillAmount = fill;
            else
                skillui.skillCool[i].fillAmount = 0;
        }
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

    public void UpgradeDamageSkill(int index)
    {
        if (PlayerManager.instance.GetMasterPlayer() != main)
            return;

        if (index < 0 || index > damageSkills.Count) return;

        DamageSkill skill = damageSkills[index];

        if (stats.skillPoint > 0 && skill.lv <= skill.maxLevel)
        {
            stats.skillPoint--;
            skill.lv++;
            Debug.Log($"{skill.name} 스킬 레벨업! 현재 레벨: {skill.lv}");
        }
        else
        {
            Debug.Log("스킬 포인트가 부족하거나 스킬이 이미 마스터 레벨입니다.");
        }
    }

    public void UpgradeBuffSkill(int index)
    {
        if (PlayerManager.instance.GetMasterPlayer() != main)
            return;

        if (index < 0 || index > buffSkills.Count) return;

        BuffSkill skill = buffSkills[index];

        if (stats.skillPoint > 0 && skill.lv <= skill.maxLevel)
        {
            stats.skillPoint--;
            skill.lv++;
            Debug.Log($"{skill.name} 스킬 레벨업! 현재 레벨: {skill.lv}");
        }
        else
        {
            Debug.Log("스킬 포인트가 부족하거나 스킬이 이미 마스터 레벨입니다.");
        }
    }

    public PlayerStats GetPlayerStats() => stats;
    public Animator GetAnimator() => animator;
}