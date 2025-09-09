using UnityEngine;

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
        if (currentCoolTime[lv] < coolTime[lv])
        {
            currentCoolTime[lv] += Time.deltaTime;
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
        if (currentCoolTime[lv] < coolTime[lv])
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
    public DamageTrigger damageTrigger;

    public override void Activate(PlayerSkill controller)
    {
        currentCoolTime[lv] = 0;
        controller.GetPlayerStats().currentMP -= needMP[lv];

        damageTrigger.SetAttack(power[lv], count[lv]);

        controller.GetAnimator().Play(animationName[lv]);
        UIManager.Instance.ShowMsg(controller.GetPlayerStats().playerType + " : " + name);
    }
}

[System.Serializable]
public class BuffSkill : BaseSkill
{
    public enum BuffType { CriticalRate, AttackSpeed }
    public BuffType buffType;
    public float[] buffValue;

    public override void Activate(PlayerSkill controller)
    {
        currentCoolTime[lv] = 0;
        controller.GetPlayerStats().currentMP -= needMP[lv];

        Debug.Log($"{name} ���� Ȱ��ȭ! ({buffType}: +{buffValue[lv]})");
        UIManager.Instance.ShowMsg(controller.GetPlayerStats().playerType + " : " + name);
    }
}