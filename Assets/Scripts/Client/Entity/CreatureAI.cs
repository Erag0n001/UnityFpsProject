using Shared;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
namespace Client
{
    public class CreatureAI : MonoBehaviour
    {
        public int creatureID;
        public Creature creature;
        public NavMeshAgent pathFinding;
        public Vector3 lastPos;
        public Quaternion lastRot;
        void Start()
        {
            creature = CreatureManager.FindCreature(creatureID);
            if (creature == null){
                Printer.LogError($"Could not find creature' {creatureID}' for {gameObject.name}, this should never happen, deleting...");
                Destroy(gameObject);
            }
            creature.creatureAI = this;
            if (MainManager.IsServer)
            {
                SetStats();
            } else 
            {
                Destroy(gameObject.GetComponent<NavMeshAgent>());
            }
        }
        //Sets initial stats
        void SetStats() 
        {
            gameObject.GetComponentInChildren<SphereCollider>().radius = creature.stats.agroRange;
            pathFinding = GetComponent<NavMeshAgent>();
            pathFinding.speed = creature.stats.baseSpeed;
            if (creature.stats.aIType == CreatureStats.AIType.Aggressive)
            {
                AgroRange();
            }
            creature.stats.status = CreatureStats.Status.Wandering;
            creature.stats.wanderingPos = Converter.Vector3UnityToVector3(transform.position);
        }
        void AgroRange()
        {
            creature.stats.agroRangeCollider = gameObject.GetComponentInChildren<SphereCollider>();
            creature.stats.agroRangeCollider.radius = creature.stats.agroRange;
        }

        void Update()
        {
            if (MainManager.IsServer)
            {
                ServerTick();
            }
            else
            {
                ClientTick();
            }
        }

        public void ServerTick() 
        {
            NeedsUpdating();
            //Check if creature was hit
            HitCheck();
            //Resets the AI if the attacker happens to die
            if (creature.stats.attacker == null)
            {
                creature.stats.status = CreatureStats.Status.Wandering;
            }
            //Set the position of the creatures in it's data container
            UpdatePos();
            //Finds location to pathfind to
            switch (creature.stats.status)
            {
                case CreatureStats.Status.Wandering: FindWanderPos(); break;
                case CreatureStats.Status.Attacking: Attack(); break;
                case CreatureStats.Status.Fleeing: Flee(); break;
            }
            //Pathfind to location
            StartPathfinding();
            //Check is creature is dead
            StatChecks();
        }

        public void NeedsUpdating() 
        {
            if(Vector3.Distance(transform.position,lastPos) > 0.1f) 
            {
                creature.stats.needsUpdating = true;
            }
            if(Quaternion.Angle(transform.rotation,lastRot) > 1) 
            {
                creature.stats.needsUpdating = true;
            }
            lastPos = transform.position;
            lastRot = transform.rotation;
        }
        public void ClientTick() 
        {
            if (creature.stats.receivedPacketMove)
            {
                creature.stats.receivedPacketMove = false;
                StartCoroutine(Lerp(Converter.Vector3ToUnityVector3(creature.stats.currentPosition)));
                StartCoroutine(LerpRot(Converter.Vector4ToQuaternion(creature.stats.currentRotation)));
            }
            if (creature.stats.receivedPacketDeath) 
            {
                DestroyCreature();
            }
        }
        public void UpdatePos()
        {
            creature.stats.currentPosition = Converter.Vector3UnityToVector3(transform.position);
            creature.stats.currentRotation = Converter.QuaternionToVector4(transform.rotation);
        }
        public void HitCheck()
        {
            if (creature.stats.hit)
            {
                switch (creature.stats.aIType)
                {
                    case CreatureStats.AIType.PassiveFlee:
                        creature.stats.status = CreatureStats.Status.Fleeing;
                        pathFinding.speed = creature.stats.sprintingSpeed;
                        break;
                    case CreatureStats.AIType.Neutral:
                        creature.stats.status = CreatureStats.Status.Attacking;
                        pathFinding.speed = creature.stats.sprintingSpeed;
                        break;
                    case CreatureStats.AIType.Aggressive:
                        creature.stats.status = CreatureStats.Status.Attacking;
                        pathFinding.speed = creature.stats.sprintingSpeed;
                        break;
                }
            }
        }

        public void StatChecks()
        {
            if (creature.stats.hitPoint <= 0)
            {
                DestroyCreature();
            }
            if (creature.stats.immunityFrames > 0)
            {
                creature.stats.immunityFrames -= 1 * Time.deltaTime;
            }
        }

        public void DestroyCreature() 
        {
            Printer.Log(gameObject.transform.position.ToString());
            Printer.Log($"Killed creature with id {creatureID} and name {this.gameObject.name}");
            MainManager.lootManager.SpawnLoot(gameObject.transform.position, creature.stats.enemyQuality);
            CreatureManager.deadCreatures.Add(creature.instanceId);
            MainManager.creatureList.Remove(creature);
            Destroy(this.gameObject);
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

        void FindWanderPos()
        {
            creature.stats.wanderingTick += 1 * Time.deltaTime;
            if (creature.stats.status == CreatureStats.Status.Wandering && creature.stats.wanderingTick >= 5)
            {
                creature.stats.wanderingPos = new SerializableVector3(UnityEngine.Random.Range(gameObject.transform.position.x - 10f, gameObject.transform.position.x + 10f), gameObject.transform.position.y, UnityEngine.Random.Range(gameObject.transform.position.z - 10f, gameObject.transform.position.z + 10f));
                creature.stats.wanderingTick = 0;
            }
        }

        public void StartPathfinding()
        {
            pathFinding.SetDestination(Converter.Vector3ToUnityVector3(creature.stats.wanderingPos));
        }

        public IEnumerator LerpRot(Quaternion rotation) 
        {
            Quaternion startRot = transform.rotation;

            float currentTime = 0f;
            float endTime = 0.250f;
            while (currentTime < endTime)
            {
                currentTime += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(startRot, rotation, currentTime / endTime);
                yield return null;
            }
            transform.rotation = rotation;
        }
        public IEnumerator Lerp(Vector3 v) 
        {
            float currentTime = 0f;
            float endTime = 0.250f;
            Vector3 startPos = gameObject.transform.position;
            while (currentTime < endTime) 
            {
                currentTime += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, v, currentTime / endTime);
                yield return null;
            }
            transform.position = v;
        }
        //Passive
        void Flee()
        {
            if (Vector3.Distance(gameObject.transform.position, creature.stats.attacker.transform.position) > 30)
            {
                creature.stats.status = CreatureStats.Status.Wandering;
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
                creature.stats.wanderingPos = Converter.Vector3UnityToVector3(targetPos);
            }
        }
        //Aggressive
        void Attack()
        {
            if (Vector3.Distance(gameObject.transform.position, creature.stats.attacker.transform.position) > 30)
            {
                creature.stats.status = CreatureStats.Status.Wandering;
                pathFinding.speed = creature.stats.baseSpeed;
            }
            else
            {
                creature.stats.wanderingPos = Converter.Vector3UnityToVector3(creature.stats.attacker.transform.position);
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
                        creature.stats.status = CreatureStats.Status.Attacking;
                        creature.stats.attacker = enemyObject;
                    }
                    if (enemyObject.CompareTag("Entity"))
                    {
                        creature.stats.isAttackerPlayer = false;
                        creature.stats.status = CreatureStats.Status.Attacking;
                        creature.stats.attacker = enemyObject;
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