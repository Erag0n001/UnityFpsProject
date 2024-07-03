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


        int selectedNumber;
        GameObject selectedPad;
        GameObject selectedCreature;
        private void Awake()
        {
            MainManager.respawnManager = this;
            creatureList = CreatureManager.creatureList;
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
            entityRespawnCooldown += 1 * Time.deltaTime;
            if (entityRespawnCooldown >= 5)
            {
                entityRespawnCooldown = 0;
                EntityRespawn();
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


        void EntityRespawn()
        {
            selectedNumber = Random.Range(0, entityRespawnPad.Length);
            selectedPad = entityRespawnPad[selectedNumber];
            selectedNumber = Random.Range(0, creatureList.Count);
            Creature creature = CreatureManager.CreateCreature(CreatureManager.creatureList[selectedNumber]);
            selectedCreature = CreatureManager.FindPrefab(creatureList[selectedNumber].uniqueName);
            selectedCreature = Object.Instantiate(selectedCreature, selectedPad.transform.position, Quaternion.identity, aliveList.transform);
            selectedCreature.GetComponent<CreatureAI>().creature = creature;
            EntitySizeRandomizer(selectedCreature);
        }

        void EntitySizeRandomizer(GameObject selectedCreature)
        {
            CreatureAI creatureAI = selectedCreature.GetComponent<CreatureAI>();
            float sizeMultiplier = Random.Range(creatureAI.creature.stats.maxSize, creatureAI.creature.stats.minSize);
            creatureAI.creature.stats.hitPoint *= sizeMultiplier;
            creatureAI.creature.stats.damage *= sizeMultiplier;
            if (creatureAI.creature.stats.aIType == CreatureStats.AIType.Aggressive)
            {
                creatureAI.creature.stats.agroRange *= sizeMultiplier;
                selectedCreature.GetComponentInChildren<SphereCollider>().radius = creatureAI.creature.stats.agroRange;
            }
            selectedCreature.GetComponent<NavMeshAgent>().speed *= sizeMultiplier;
            selectedCreature.transform.localScale *= sizeMultiplier;
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