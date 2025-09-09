using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerMagic : PlayerMovement
{
    public override void Attack(GameObject target)
    {
        PlayerStats stats = GetComponent<PlayerStats>();
        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            lastAttackTime = Time.time;
            gameObject.GetComponentInChildren<Animator>().Play("Attack");
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<Enemy>().isDie) continue;
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist <= stats.attackRange)
                {
                    Enemy targetStats = enemy.GetComponent<Enemy>();
                    int damage = stats.attackPower;

                    if (Random.Range(0f, 100f) <= stats.criticalChance)
                        damage = Mathf.RoundToInt(damage * 1.5f);

                    targetStats.TakeDamage(damage);

                    #region °æÇèÄ¡
                    float value;
                    if (damage > targetStats.hp)
                        value = targetStats.haveExp * damage / targetStats.hpMax;
                    else
                        value = targetStats.haveExp * targetStats.hp / targetStats.hpMax;

                    stats.GetExp(value);
                    #endregion
                }
            }
        }
    }
}
