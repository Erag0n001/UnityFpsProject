using Shared;
using UnityEngine;
using UnityEngine.AI;
namespace Client
{
    public class CreatureAI : MonoBehaviour
    {
        public Creature creature;
        public NavMeshAgent pathFinding;
        void Start()
        {
            pathFinding = GetComponent<NavMeshAgent>();
            pathFinding.speed = creature.stats.baseSpeed;
            if (creature.stats.aIType == CreatureStats.AIType.Aggressive)
            {
                AgroRange();
            }
            creature.stats.wandering = true;
        }

        void AgroRange()
        {
            creature.stats.agroRangeCollider = gameObject.GetComponentInChildren<SphereCollider>();
            creature.stats.agroRangeCollider.radius = creature.stats.agroRange;
        }

        void Update()
        {
            //Check if creature was hit
            HitCheck();
            //Resets the AI if the attacker happens to die
            if (creature.stats.attacker == null)
            {
                creature.stats.fleeing = false;
                creature.stats.aggressive = false;
                creature.stats.wandering = true;
            }
            //Wanders creature around
            Wander();
            if (creature.stats.aggressive)
            {
                Attack();
            }
            if (creature.stats.fleeing)
            {
                Flee();
            }
            //Check is creature is dead
            StatChecks();
        }

        public void HitCheck()
        {
            if (creature.stats.hit)
            {
                switch (creature.stats.aIType)
                {
                    case CreatureStats.AIType.PassiveFlee:
                        creature.stats.fleeing = true;
                        pathFinding.speed = creature.stats.sprintingSpeed;
                        break;
                    case CreatureStats.AIType.Neutral:
                        creature.stats.aggressive = true;
                        pathFinding.speed = creature.stats.sprintingSpeed;
                        break;
                    case CreatureStats.AIType.Aggressive:
                        creature.stats.aggressive = true;
                        pathFinding.speed = creature.stats.sprintingSpeed;
                        break;
                }
            }
        }

        public void StatChecks()
        {
            if (creature.stats.hitPoint <= 0)
            {
                MainManager.lootManager.SpawnLoot(gameObject.transform.position, creature.stats.enemyQuality);
                Destroy(gameObject);
            }
            if (creature.stats.immunityFrames > 0)
            {
                creature.stats.immunityFrames -= 1 * Time.deltaTime;
            }
        }
        public void AddHP(float modifier)
        {
            creature.stats.hitPoint += modifier;
            if (modifier > 0)
            {
                creature.stats.hit = true;
                creature.stats.immunityFrames += modifier * 0.1f * -1;
            }
        }

        void Wander()
        {
            creature.stats.wanderingTick += 1 * Time.deltaTime;
            if (creature.stats.wandering && creature.stats.wanderingTick >= 5)
            {
                creature.stats.wanderingPos = new Vector3(UnityEngine.Random.Range(gameObject.transform.position.x - 10f, gameObject.transform.position.x + 10f), gameObject.transform.position.y, UnityEngine.Random.Range(gameObject.transform.position.z - 10f, gameObject.transform.position.z + 10f));
                pathFinding.SetDestination(creature.stats.wanderingPos);
                creature.stats.wanderingTick = 0;
            }
        }
        //Passive
        void Flee()
        {
            if (Vector3.Distance(gameObject.transform.position, creature.stats.attacker.transform.position) > 30)
            {
                creature.stats.fleeing = false;
                creature.stats.wandering = true;
                pathFinding.speed = creature.stats.baseSpeed;
            }
            else
            {
                Vector3 targetPos = new Vector3();
                if (creature.stats.isAttackerPlayer)
                {
                    gameObject.transform.forward = MainManager.PlayerRot * Vector3.forward;
                }
                else
                {
                    gameObject.transform.forward = creature.stats.attacker.transform.forward;
                }
                targetPos = gameObject.transform.position + gameObject.transform.forward * 5;
                pathFinding.SetDestination(targetPos);
            }
        }
        //Aggressive
        void Attack()
        {
            if (Vector3.Distance(gameObject.transform.position, creature.stats.attacker.transform.position) > 30)
            {
                creature.stats.aggressive = false;
                creature.stats.wandering = true;
                pathFinding.speed = creature.stats.baseSpeed;
            }
            else
            {
                pathFinding.SetDestination(creature.stats.attacker.transform.position);
            }
        }

        public void SetAgro(GameObject enemyObject)
        {
            {
                if (creature.stats.aIType == CreatureStats.AIType.Aggressive)
                {
                    if (enemyObject.tag == "Player")
                    {
                        creature.stats.isAttackerPlayer = true;
                        creature.stats.aggressive = true;
                        creature.stats.attacker = enemyObject;
                        creature.stats.wandering = false;
                    }
                    if (enemyObject.CompareTag("Entity"))
                    {
                        creature.stats.isAttackerPlayer = false;
                        creature.stats.aggressive = true;
                        creature.stats.attacker = enemyObject;
                        creature.stats.wandering = false;
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
            if (other.tag == "Player" && creature.stats.immunityFrames <= 0)
            {
                creature.stats.isAttackerPlayer = true;
                creature.stats.attacker = other.gameObject;
                PlayerStatManager.Stats enemyStats = creature.stats.attacker.GetComponent<PlayerStatManager>().stats;
                if (enemyStats.damage != 0)
                {
                    AddHP(enemyStats.damage * -1);
                }
            }
            if (other.tag == "Entity" && creature.stats.immunityFrames <= 0)
            {
                creature.stats.isAttackerPlayer = false;
                creature.stats.attacker = other.gameObject;
                CreatureStats enemyStats = creature.stats.attacker.GetComponent<CreatureAI>().creature.stats;
                AddHP(enemyStats.damage * -1);
            }
        }
    }
}