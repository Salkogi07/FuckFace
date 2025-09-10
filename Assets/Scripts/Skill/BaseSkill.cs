using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Playables;

[System.Serializable]
public abstract class BaseSkill
{
    public string name;
    public int lv;

    public float[] coolTime;
    public float[] currentCoolTime;
    public float[] needMP;

    public virtual void Tick()
    {
        if (lv <= 0) return;
        if (currentCoolTime[lv] > 0)
        {
            currentCoolTime[lv] -= Time.deltaTime;
        }
    }

    public virtual bool CanUse(PlayerStats stats)
    {
        if (lv <= 0)
        {
            Debug.LogWarning($"{name} ��ų�� ������ 0 ���Ͽ��� ����� �� �����ϴ�.");
            return false;
        }
        if (stats.currentMP < needMP[lv])
        {
            Debug.Log("������ �����մϴ�.");
            return false;
        }
        if (currentCoolTime[lv] > 0)
        {
            Debug.Log("��Ÿ�� ���Դϴ�.");
            return false;
        }
        return true;
    }

    public abstract void Activate(PlayerSkill controller);
}

[System.Serializable]
public class DamageSkill : BaseSkill
{
    public float[] power;
    public int[] count;
    public string[] animationName;
    public TriggerDamageEnemy damageTrigger;

    public override void Activate(PlayerSkill controller)
    {
        currentCoolTime[lv] = coolTime[lv];
        controller.GetPlayerStats().currentMP -= needMP[lv];

        damageTrigger.SetAttack(power[lv], count[lv]);

        controller.GetAnimator().Play(animationName[lv]);
        UIManager.Instance.ShowMsg(controller.GetPlayerStats().playerType + " : " + name);
    }
}

[System.Serializable]
public class BuffSkill : BaseSkill
{
    public enum BuffType { CriticalRate, AttackSpeed, Health, MoveSpeed, Damage }
    public BuffType buffType;
    public float[] buffValue;
    public ParticleSystem partical;

    public override void Activate(PlayerSkill controller)
    {
        currentCoolTime[lv] = coolTime[lv];
        controller.GetPlayerStats().currentMP -= needMP[lv];
        controller.StartCoroutine(Buffer(controller,3));

        UIManager.Instance.ShowMsg(controller.GetPlayerStats().playerType + " : " + name);
    }

    #region ������ ����
    public IEnumerator Buffer(PlayerSkill controller, float value)
    {
        PlayerMovement playerM = controller.main.GetComponent<PlayerMovement>();
        PlayerStats playerS = controller.main.GetComponent<PlayerStats>();

        float origin = 0;

        switch (buffType)
        {
            case BuffType.CriticalRate:
                partical.Play();
                origin = playerS.criticalChance;
                playerS.criticalChance += buffValue[lv];
                yield return new WaitForSeconds(value);
                playerS.criticalChance = origin;
                partical.Stop();
                break;
            case BuffType.AttackSpeed:
                partical.Play();
                origin = playerS.attackCooldown;
                playerS.attackCooldown += buffValue[lv];
                yield return new WaitForSeconds(value);
                playerS.attackCooldown = origin;
                partical.Stop();
                break;
            case BuffType.Health:
                partical.Play();
                origin = playerS.currentHP;
                playerS .currentHP += buffValue[lv];
                yield return new WaitForSeconds(value);
                playerS.attackCooldown = origin;
                partical.Stop();
                break;
            case BuffType.Damage:
                partical.Play();
                origin = playerS.attackPower;
                playerS.attackPower += buffValue[lv];
                yield return new WaitForSeconds(value);
                playerS.attackCooldown = origin;
                partical.Stop();
                break;
            case BuffType.MoveSpeed:
                partical.Play();
                origin = playerM.moveSpeed;
                playerM.moveSpeed += buffValue[lv];
                yield return new WaitForSeconds(value);
                playerS.attackCooldown = origin;
                partical.Stop();
                break;
            default:
                yield return null;
                break;
        }
    }
    #endregion
}