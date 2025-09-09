using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamagePlayer : MonoBehaviour
{
    public LayerMask targetLayer;
    private BoxCollider boxCollider;

    public float damage;
    public int count;

    public int i = 0;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        AttackInBox();
    }

    private void AttackInBox()
    {
        Vector3 center = boxCollider.bounds.center;
        Vector3 halfExtents = boxCollider.bounds.extents;
        Collider[] hits = Physics.OverlapBox(center, halfExtents, transform.rotation, targetLayer);

        HashSet<PlayerStats> uniqueEnemies = new HashSet<PlayerStats>();

        foreach (var hit in hits)
        {
            PlayerStats enemy = hit.GetComponentInParent<PlayerStats>();
            if (enemy != null)
            {
                uniqueEnemies.Add(enemy);
            }
        }

        int attackCount = Mathf.Min(count, uniqueEnemies.Count);

        foreach (PlayerStats enemy in uniqueEnemies)
        {
            if (i >= attackCount) break;
            enemy.TakeDamage(damage);
            i++;
        }
    }

    public void SetAttack(float damage, int count)
    {
        i = 0;
        this.damage = damage;
        this.count = count;
    }
}
