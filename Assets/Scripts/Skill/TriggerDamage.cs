using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamage : MonoBehaviour
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

        HashSet<Enemy> uniqueEnemies = new HashSet<Enemy>();

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                uniqueEnemies.Add(enemy);
            }
        }

        int attackCount = Mathf.Min(count, uniqueEnemies.Count);

        foreach (Enemy enemy in uniqueEnemies)
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
