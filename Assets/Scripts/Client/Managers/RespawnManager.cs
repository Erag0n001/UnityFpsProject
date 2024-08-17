using System.Collections.Generic;
using Shared;
using UnityEngine;
using UnityEngine.AI;
namespace Client
{
    public class RespawnManager : MonoBehaviour
    {
        private List<Creature> creatureList = new List<Creature>();
        private GameObject playerPrefab;
        private GameObject[] entityRespawnPad;
        private GameObject[] playerRespawnPads;
        private GameObject aliveList;
        private float entityRespawnCooldown;
        private float playerRespawnCooldown;


        private void Awake()
        {
            MainManager.respawnManager = this;
            creatureList = Shared.CreatureManager.creatureList;
            playerPrefab = Resources.Load<GameObject>("Prefab/Entities/Player/Player");
        }

        void Start()
        {
            aliveList = GameObject.Find("Alive");
            entityRespawnPad = GameObject.FindGameObjectsWithTag("EntityRespawnPad");
            playerRespawnPads = GameObject.FindGameObjectsWithTag("PlayerRespawnPad");
            playerRespawn();
        }
        void Update()
        {
            if (MainManager.IsServer) 
            {
                entityRespawnCooldown += 1 * Time.deltaTime;
                if (entityRespawnCooldown >= 5)
                {
                    entityRespawnCooldown = 0;
                    EntityRespawn();
                }
            }
            if (!MainManager.isPlayerAlive)
            {
                playerRespawnCooldown += 1 * Time.deltaTime;
                if (playerRespawnCooldown >= 5)
                {
                    playerRespawn();
                    playerRespawnCooldown = 0;
                }
            }
        }


        public void EntityRespawn(Creature creature = null, Vector3? spawnPosition = null)
        {
            int selectedNumber;
            int id = 0;
            if (creature == null)
            {
                selectedNumber = Random.Range(0, creatureList.Count);
                creature = Shared.CreatureManager.CreateCreature(Shared.CreatureManager.creatureList[selectedNumber]);
            } else 
            {
                id = creature.instanceId;
            }

            if (spawnPosition == null)
            {
                selectedNumber = Random.Range(0, entityRespawnPad.Length);
                spawnPosition = entityRespawnPad[selectedNumber].transform.position;
            }
            if (MainManager.IsServer)
            {
                MainManager.currentCreatureIDCount += 1;
                creature.instanceId = MainManager.currentCreatureIDCount;
                id = MainManager.currentCreatureIDCount;
            }
            MainManager.creatureList.Add(creature);
            MainManager.unityMainThreadDispatcher.Enqueue(() => 
            {
                SpawnCreature(creature, spawnPosition.Value);
            });
        }

        public void SpawnCreature(Creature creature, Vector3 spawnPosition) 
        {
            GameObject selectedCreature = Shared.CreatureManager.FindPrefab(creature.uniqueName);
            CreatureAI creatureAI = selectedCreature.GetComponent<CreatureAI>();
            creatureAI.creatureID = creature.instanceId;
            selectedCreature = UnityEngine.Object.Instantiate(selectedCreature, spawnPosition, Quaternion.identity, aliveList.transform);
            selectedCreature.name = creature.uniqueName + " " + creature.instanceId;
        }
        void CreatureStatRandomizer(Creature creature)
        {
            float statMultiplier = Random.Range(creature.stats.maxSize, creature.stats.minSize);
            creature.stats.hitPoint *= statMultiplier;
            creature.stats.damage *= statMultiplier;
            if (creature.stats.aIType == CreatureStats.AIType.Aggressive)
            {
                creature.stats.agroRange *= statMultiplier;
            }
        }
        void playerRespawn()
        {
            MainManager.alivePlayer = Object.Instantiate(playerPrefab, playerRespawnPads[Random.Range(0, playerRespawnPads.Length)].transform.position, Quaternion.identity, aliveList.transform);
            MainManager.playerStatManager = MainManager.alivePlayer.gameObject.GetComponent<PlayerStatManager>();
            MainManager.playerMovement = MainManager.alivePlayer.gameObject.GetComponent<CharacterMovement>();
            MainManager.isPlayerAlive = true;
            Camera camera = MainManager.alivePlayer.transform.Find("Head").Find("Camera").gameObject.GetComponent<Camera>();
            camera.enabled = true;
            if (MainManager.latestPlayerDeadBody != null)
            {
                GameObject deadCamera = MainManager.latestPlayerDeadBody.transform.Find("PlayerDeadCamera").gameObject;
                GameObject.Destroy(deadCamera);
            }
        }
    }
}