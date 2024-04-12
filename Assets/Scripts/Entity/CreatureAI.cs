using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
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
        public float baseSpeed;
        public float sprintingSpeed;
        [NonSerialized] public float immunityFrames;
    }
    public Stats stats;

    [Header("AI")]
    [SerializeField] public AIType aIType;
    public enum AIType {PassiveFlee, Passive, Neutral, Aggressive};
    private bool hit;
    public NavMeshAgent pathFinding; 
    GameObject attacker;

    [Header("Wandering")]
    private bool wandering;
    private Vector3 wanderingPos;
    private float wanderingTick;

    [Header("PassiveFlee")]
    private bool fleeing;  

    [Header("Aggressive")]
    private bool aggressive;
    private SphereCollider agroRangeCollider;
    void Start()
    {
        pathFinding.speed = stats.baseSpeed;
        if(aIType == AIType.Aggressive)
        {
            AgroRange();
        }
        wandering = true;
    }

    void AgroRange()
    {
        agroRangeCollider = gameObject.GetComponentInChildren<SphereCollider>();
        agroRangeCollider.radius = stats.agroRange;
    }

    void Update()
    {
        //Check if creature was hit
        HitCheck();
        //Resets the AI if the attacker happens to die
        if(attacker == null)
        {
            fleeing = false;
            aggressive = false;
            wandering = true;
        }
        //Check is creature is dead
        //Wanders creature around
        Wander();
        if(aggressive)
        {
            Attack();
        }
        if(fleeing)
        {
            Flee();
        }
        StatChecks();
    }

    public void HitCheck()
    {
        if(hit)
        {
            switch (aIType)
            {
                case AIType.PassiveFlee:
                fleeing = true;
                pathFinding.speed = stats.sprintingSpeed;
                break;
                case AIType.Neutral:
                aggressive = true;
                pathFinding.speed = stats.sprintingSpeed;
                break;
                case AIType.Aggressive:
                aggressive = true;
                pathFinding.speed = stats.sprintingSpeed;
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
    public void AddHP(float modifier, GameObject attacker) 
    { 
        stats.hitPoint += modifier; 
        if(modifier < 0)
        {
            hit = true;
            this.attacker = attacker;
            stats.immunityFrames += modifier * 0.1f * -1;
        }
    }

    void Wander()
    {
        wanderingTick += 1 * Time.deltaTime;
        if (wandering && wanderingTick >= 5)
        {
            wanderingPos = new Vector3(UnityEngine.Random.Range(gameObject.transform.position.x - 10f, gameObject.transform.position.x + 10f), gameObject.transform.position.y, UnityEngine.Random.Range(gameObject.transform.position.z - 10f, gameObject.transform.position.z + 10f));
            pathFinding.SetDestination(wanderingPos);
            wanderingTick = 0;
        }
    }
    //Passive
    void Flee()
    {
        if (Vector3.Distance(gameObject.transform.position, attacker.transform.position) > 12)
        {
            fleeing = false;
            wandering = true;
            pathFinding.speed = stats.baseSpeed;
        } else
        {
            Vector3 targetPos = new Vector3();
            gameObject.transform.forward = attacker.transform.forward;
            targetPos = gameObject.transform.position + gameObject.transform.forward * 5;
            print(targetPos);
            pathFinding.SetDestination(targetPos);
        }
    }
    //Aggressive
    void Attack()
    {
        if (Vector3.Distance(gameObject.transform.position, attacker.transform.position) > 12)
        {
            aggressive = false;
            wandering = true;
            pathFinding.speed = stats.baseSpeed;
        } else
        {
            if(pathFinding)
            pathFinding.SetDestination(attacker.transform.position);
        }
    }

    public void SetAgro(GameObject enemyObject)
    {
        if(enemyObject != gameObject)
        {
            if(aIType == AIType.Aggressive)
            {
                if (enemyObject.tag == "Player" || enemyObject.tag == "Entity")
                {
                    aggressive = true;
                    attacker = enemyObject;
                    wandering = false;
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
            attacker = other.gameObject;
            PlayerStatManager.Stats enemyStats;
            enemyStats = attacker.GetComponent<PlayerStatManager>().stats;
            if (enemyStats.damage != 0)
            {
                AddHP(enemyStats.damage * -1,attacker);
            }
        }
        if (other.tag == "Entity" && stats.immunityFrames <= 0)
        {
            attacker = other.gameObject;
            CreatureAI.Stats enemyStats;
            enemyStats = attacker.GetComponent<CreatureAI>().stats;
            AddHP(enemyStats.damage * -1,attacker);
        }
    }
}
