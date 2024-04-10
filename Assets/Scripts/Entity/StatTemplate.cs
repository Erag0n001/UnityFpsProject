using System;
using UnityEngine;

public class StatTemplate : MonoBehaviour
{
    [Serializable]
    public class Stats
    {
        public float hitPoint;
        public float damage;
        public float agroRange;
        public int enemyQuality;
        public float minSize;
        public float maxSize;
        [NonSerialized] public float immunityFrames;
    }
    private GameObject enemy;

    private Vector3 knockbackVector;
    private Rigidbody rigidBody;
    private SphereCollider agroRangeCollider;

    private PlayerStatManager.Stats enemyStats;
    private PlayerStatManager playerStatManager;
    public Stats stats;
    void Start()
    {
        AgroRange();
    }

    void AgroRange()
    {
        agroRangeCollider = gameObject.GetComponentInChildren<SphereCollider>();
        agroRangeCollider.radius = stats.agroRange;
    }

    void Update()
    {
        StatChecks();
    }

    void OnTriggerStay(Collider other)
    {
        enemy = other.gameObject;
        DamageCheck();
    }
    void DamageCheck()
    {
        if (enemy.tag == "Player" && stats.immunityFrames <= 0)
        {
            playerStatManager = enemy.GetComponent<PlayerStatManager>();
            enemyStats = playerStatManager.stats;
            if (enemyStats.damage != 0)
            {
                AddHP(enemyStats.damage * -1);
                SetImmunityFrames(0.1f);
            }
        }
        else if (enemy.tag == "Enemy")
        {

        }
    }
    public void StatChecks()
    {
        if (stats.hitPoint <= 0)
        {
            MainManager.lootManager.SpawnLoot(gameObject.transform.position, stats.enemyQuality);
            Destroy(gameObject);
        }
        if (stats.immunityFrames > 0)
        {
            SetImmunityFrames(stats.immunityFrames - 1 * Time.deltaTime);
        }
    }
    public void AddHP(float modifier) { stats.hitPoint += modifier; }
    public void SetImmunityFrames(float modifier) { stats.immunityFrames = modifier; }
}