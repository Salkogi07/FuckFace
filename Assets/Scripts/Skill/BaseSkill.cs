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
            Debug.LogWarning($"{name} 스킬의 레벨이 0 이하여서 사용할 수 없습니다.");
            return false;
        }
        if (stats.currentMP < needMP[lv])
        {
            Debug.Log("마나가 부족합니다.");
            return false;
        }
        if (currentCoolTime[lv] < coolTime[lv])
        {
            Debug.Log("쿨타임 중입니다.");
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

        Debug.Log($"{name} 버프 활성화! ({buffType}: +{buffValue[lv]})");
        UIManager.Instance.ShowMsg(controller.GetPlayerStats().playerType + " : " + name);
    }
}