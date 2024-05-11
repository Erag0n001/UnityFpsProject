using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{ 
    [Serializable]
    public class CreatureStat
    {
        [Header("Stats")]
        public float hitPoint;
        public float damage;
        public float agroRange;
        public int enemyQuality;
        public float minSize;
        public float maxSize;
        public float baseSpeed;
        public float sprintingSpeed;
        [HideInInspector] public float immunityFrames;

        [Header("AI")]
        [SerializeField] public AIType aIType;
        public enum AIType {PassiveFlee, Passive, Neutral, Aggressive};
        [HideInInspector] public bool hit;
        [HideInInspector] public bool isAttackerPlayer;
        [HideInInspector] public NavMeshAgent pathFinding; 
        [HideInInspector] public GameObject attacker;

        [Header("Wandering")]
        [HideInInspector] public bool wandering;
        [HideInInspector] public Vector3 wanderingPos;
        [HideInInspector] public float wanderingTick;

        [Header("PassiveFlee")]
        [HideInInspector] public bool fleeing;  

        [Header("Aggressive")]
        [HideInInspector] public bool aggressive;
        public SphereCollider agroRangeCollider;
    }
    public CreatureStat stats;

    void Start()
    {
        stats.pathFinding = GetComponent<NavMeshAgent>();
        stats.pathFinding.speed = stats.baseSpeed;
        if(stats.aIType == CreatureStat.AIType.Aggressive)
        {
            AgroRange();
        }
        stats.wandering = true;
    }

    void AgroRange()
    {
        stats.agroRangeCollider = gameObject.GetComponentInChildren<SphereCollider>();
        stats.agroRangeCollider.radius = stats.agroRange;
    }

    void Update()
    {
        //Check if creature was hit
        HitCheck();
        //Resets the AI if the attacker happens to die
        if(stats.attacker == null)
        {
            stats.fleeing = false;
            stats.aggressive = false;
            stats.wandering = true;
        }
        //Wanders creature around
        Wander();
        if(stats.aggressive)
        {
            Attack();
        }
        if(stats.fleeing)
        {
            Flee();
        }
        //Check is creature is dead
        StatChecks();
    }

    public void HitCheck()
    {
        if(stats.hit)
        {
            switch (stats.aIType)
            {
                case CreatureStat.AIType.PassiveFlee:
                stats.fleeing = true;
                stats.pathFinding.speed = stats.sprintingSpeed;
                break;
                case CreatureStat.AIType.Neutral:
                stats.aggressive = true;
                stats.pathFinding.speed = stats.sprintingSpeed;
                break;
                case CreatureStat.AIType.Aggressive:
                stats.aggressive = true;
                stats.pathFinding.speed = stats.sprintingSpeed;
                break;
            }
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
            stats.immunityFrames -= 1 * Time.deltaTime;
        }
    }
    public void AddHP(float modifier) 
    { 
        stats.hitPoint += modifier; 
        if(modifier > 0)
        {
            stats.hit = true;
            stats.immunityFrames += modifier * 0.1f * -1;
        }
    }

    void Wander()
    {
        stats.wanderingTick += 1 * Time.deltaTime;
        if (stats.wandering && stats.wanderingTick >= 5)
        {
            stats.wanderingPos = new Vector3(UnityEngine.Random.Range(gameObject.transform.position.x - 10f, gameObject.transform.position.x + 10f), gameObject.transform.position.y, UnityEngine.Random.Range(gameObject.transform.position.z - 10f, gameObject.transform.position.z + 10f));
            stats.pathFinding.SetDestination(stats.wanderingPos);
            stats.wanderingTick = 0;
        }
    }
    //Passive
    void Flee()
    {
        if (Vector3.Distance(gameObject.transform.position, stats.attacker.transform.position) > 30)
        {
            stats.fleeing = false;
            stats.wandering = true;
            stats.pathFinding.speed = stats.baseSpeed;
        } 
        else
        {
            Vector3 targetPos = new Vector3();
            if(stats.isAttackerPlayer)
            {
                gameObject.transform.forward = MainManager.PlayerRot * Vector3.forward;
            } 
            else 
            {
                gameObject.transform.forward = stats.attacker.transform.forward;
            }
            targetPos = gameObject.transform.position + gameObject.transform.forward * 5;
            stats.pathFinding.SetDestination(targetPos);
        }
    }
    //Aggressive
    void Attack()
    {
        if (Vector3.Distance(gameObject.transform.position, stats.attacker.transform.position) > 30)
        {
            stats.aggressive = false;
            stats.wandering = true;
            stats.pathFinding.speed = stats.baseSpeed;
        } else
        {
            stats.pathFinding.SetDestination(stats.attacker.transform.position);
        }
    }

    public void SetAgro(GameObject enemyObject)
    {
        {
            if(stats.aIType == CreatureStat.AIType.Aggressive)
            {
                if (enemyObject.tag == "Player")
                {
                    stats.isAttackerPlayer = true;
                    stats.aggressive = true;
                    stats.attacker = enemyObject;
                    stats.wandering = false;
                }
                if (enemyObject.CompareTag("Entity"))
                {
                    stats.isAttackerPlayer = false;
                    stats.aggressive = true;
                    stats.attacker = enemyObject;
                    stats.wandering = false;
                }
            }
        }
    }
    //Damage detection
    void OnTriggerStay(Collider other)
    {
        DamageCheck(other);
    }
    void DamageCheck(Collider other)
    {
        if (other.tag == "Player" && stats.immunityFrames <= 0)
        {
            PlayerStatManager.Stats enemyStats;
            stats.isAttackerPlayer = true;
            stats.attacker = other.gameObject;
            enemyStats = stats.attacker.GetComponent<PlayerStatManager>().stats;
            if (enemyStats.damage != 0)
            {
                AddHP(enemyStats.damage * -1);
            }
        }
        if (other.tag == "Entity" && stats.immunityFrames <= 0)
        {
            CreatureAI.CreatureStat enemyStats;
            stats.isAttackerPlayer = false;
            stats.attacker = other.gameObject;
            enemyStats = stats.attacker.GetComponent<CreatureAI>().stats;
            AddHP(enemyStats.damage * -1);
        }
    }
}
