using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public enum PlayerType { Melee, Ranged, Magic }

public class PlayerStats : MonoBehaviour
{
    public PlayerType playerType;

    public Image hpImage;
    public Text hpText;

    public Image mpImage;
    public Text mpText;

    public Image expImage;
    public Text expText;

    public Text levelText;

    public float maxHP, currentHP;

    public float maxMP, currentMP;

    public float maxExp, currentExp;

    public int attackPower, attackRange;
        
    public float attackCooldown, criticalChance;

    public int level = 0;
    public int skillPoint = 1;
    public int hpUp;
    public int damageUp;

    public bool isGod = false;
    public bool isDie = false;
    public bool isDamage = false;

    private void Awake()
    {
        currentHP = maxHP;
        currentMP = maxMP;
        currentExp = maxExp;

        attackRange = attackRange * 3;
    }

    private void Update()
    {
        hpImage.fillAmount = currentHP / maxHP;
        hpText.text = currentHP.ToString() + "/" + maxHP.ToString();
        mpImage.fillAmount = currentMP / maxMP;
        mpText.text = currentMP.ToString() + "/" + maxMP.ToString();
        expImage.fillAmount = currentExp / maxExp;
        expText.text = currentExp.ToString() + "/" + maxExp.ToString();

        levelText.text = level.ToString();

        if (!isDamage)
        {
            RegenerateHP(1f, Time.deltaTime);
            RegenerateMP(5, Time.deltaTime);
        }
    }

    #region 레벨 업
    public void GetExp(float value)
    {
        currentExp += value;
        if(currentExp >= maxExp)
        {
            currentExp -= maxExp;
            LevelUp();
        }
    }

    public void LevelUp()
    {
        if (isDie)
            return;

        level++;
        if(level > 20)
            level = 20;

        if (level % 2 == 0)
            skillPoint++;

        maxExp *= 1.5f;

        maxHP += hpUp;
        attackPower += damageUp;
        currentHP = maxHP;

        UIManager.Instance.ShowMsg(playerType +" : Lv."+level + " 로 레벨업");
    }
    #endregion

    public void TakeDamage(float damage)
    {
        if (isDie)
            return;

        currentHP -= damage;
        StartCoroutine(dm());
        if (currentHP <= 0)
        {
            UIManager.Instance.ShowMsgBig(playerType+" : 사망!");
            gameObject.SetActive(false);
            currentHP = 0;
            isDie = true;
        }
    }

    private IEnumerator dm()
    {
        isDamage = true;

        yield return new WaitForSeconds(5);

        isDamage = false;
    }

    // 수정된 HP 회복 함수
    public void RegenerateHP(float amountPerSecond, float deltaTime)
    {
        currentHP += amountPerSecond * deltaTime;

        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
    }

    // 수정된 MP 회복 함수
    public void RegenerateMP(float amountPerSecond, float deltaTime)
    {
        currentMP += amountPerSecond * deltaTime;

        currentMP = Mathf.Clamp(currentMP, 0, maxMP);
    }
}