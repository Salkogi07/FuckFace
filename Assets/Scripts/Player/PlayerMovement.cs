using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;
    public LayerMask groundLayer;

    [Header("AI 설정")]
    public bool isAI = true;
    public float followDistance = 3f;
    public float sightRange = 10f;

    [Header("간격 조절")]
    public float minDistanceBetweenPlayers = 3f;
    public float spacingForce = 2f;

    private Rigidbody rb;
    private Animator animator;
    private PlayerStats stats;

    private Vector3 targetPosition;
    private bool hasTarget = false;
    private GameObject currentEnemy;
    protected float lastAttackTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        stats = GetComponent<PlayerStats>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (stats.isDie || !GameManager.Instance.isStart) return;

        GameObject masterPlayer = PlayerManager.instance.GetMasterPlayer();

        if (isAI)
        {
            FindNearestEnemy();

            if (currentEnemy != null)
            {
                float distToEnemy = Vector3.Distance(transform.position, currentEnemy.transform.position);
                if (distToEnemy <= stats.attackRange)
                {
                    hasTarget = false;
                    rb.velocity = Vector3.zero;

                    Attack(currentEnemy);
                }
                else
                {
                    animator.Play("Move");
                    targetPosition = currentEnemy.transform.position;
                    hasTarget = true;
                }
            }
            else if (masterPlayer != null)
            {
                float distToMaster = Vector3.Distance(transform.position, masterPlayer.transform.position);
                if (distToMaster > followDistance)
                {
                    animator.Play("Move");
                    targetPosition = masterPlayer.transform.position;
                    hasTarget = true;
                }
                else
                {
                    animator.Play("Idle");
                    hasTarget = false;
                    rb.velocity = Vector3.zero;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
                {
                    animator.Play("Move");
                    targetPosition = hit.point;
                    hasTarget = true;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                FindNearestEnemy();

                if (currentEnemy != null)
                {
                    float distToEnemy = Vector3.Distance(transform.position, currentEnemy.transform.position);
                    if (distToEnemy <= stats.attackRange)
                    {
                        hasTarget = false;
                        rb.velocity = Vector3.zero;

                        Attack(currentEnemy);
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (hasTarget)
        {
            Vector3 direction = (targetPosition - rb.position);
            direction.y = 0;
            direction.Normalize();

            // AI가 마스터 플레이어를 따라가는 중일 때만 간격 조절 적용
            Vector3 finalDirection = direction;
            if (isAI && currentEnemy == null && PlayerManager.instance.GetMasterPlayer() != null)
            {
                Vector3 spacingOffset = CalculateSpacingOffset();
                finalDirection = (direction + spacingOffset).normalized;
            }


            if (Vector3.Distance(new Vector3(rb.position.x, 0, rb.position.z),
                new Vector3(targetPosition.x, 0, targetPosition.z)) < 0.5f)
            {
                animator.Play("Idle");
                hasTarget = false;
            }
            else
            {
                animator.Play("Move");
                rb.velocity = new Vector3(finalDirection.x * moveSpeed, rb.velocity.y, finalDirection.z * moveSpeed);
                transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            }
        }
    }

    Vector3 CalculateSpacingOffset()
    {
        Vector3 spacingOffset = Vector3.zero;
        GameObject[] allPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject otherPlayer in allPlayers)
        {
            if (otherPlayer == this.gameObject) continue;

            Vector3 toOther = otherPlayer.transform.position - transform.position;
            toOther.y = 0; // Y축 무시
            float distance = toOther.magnitude;

            if (distance < minDistanceBetweenPlayers && distance > 0.1f)
            {
                Vector3 avoidDirection = -toOther.normalized;
                float pushStrength = (minDistanceBetweenPlayers - distance) / minDistanceBetweenPlayers;
                spacingOffset += avoidDirection * spacingForce * pushStrength;
            }
        }

        return spacingOffset;
    }

    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float nearestDist = Mathf.Infinity;
        currentEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<Enemy>().isDie) continue;
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < nearestDist && dist <= sightRange)
            {
                nearestDist = dist;
                currentEnemy = enemy;
            }
        }
    }

    public virtual void Attack(GameObject target)
    {
        if (Time.time >= lastAttackTime + stats.attackCooldown)
        {
            lastAttackTime = Time.time;
            animator.Play("Attack");
            transform.LookAt(new Vector3(currentEnemy.transform.position.x, transform.position.y, currentEnemy.transform.position.z));
            Enemy targetStats = target.GetComponent<Enemy>();
            int damage = stats.attackPower;

            if (Random.Range(0f, 100f) <= stats.criticalChance)
                damage = Mathf.RoundToInt(damage * 1.5f);

            targetStats.TakeDamage(damage);

            #region 경험치
            float value;
            if (damage > targetStats.hp)
                value = targetStats.haveExp * damage / targetStats.hpMax;
            else
                value = targetStats.haveExp * targetStats.hp / targetStats.hpMax;

            stats.GetExp(value);
            #endregion

            if (targetStats.isDie)
                currentEnemy = null;
        }
    }
}