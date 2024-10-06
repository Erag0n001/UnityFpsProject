using System.Collections.Generic;
using System.Threading;
using System.Collections;
using Shared;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
namespace Client
{
    public class RespawnManager : MonoBehaviour
    {
        private List<Creature> creatureList = new List<Creature>();
        private GameObject playerPrefab;
        private GameObject fakePlayerPrefab;
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
            fakePlayerPrefab = Resources.Load<GameObject>("Prefab/Entities/Player/FakePlayer");
        }

        void Start()
        {
            aliveList = GameObject.Find("Alive");
            entityRespawnPad = GameObject.FindGameObjectsWithTag("EntityRespawnPad");
            playerRespawnPads = GameObject.FindGameObjectsWithTag("PlayerRespawnPad");
            MainManager.Socializing.AddToQueue(new Packet("RequestPlayerBody", null));
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

        public IEnumerator PlayerRespawnCooldown(Player player) 
        {
            Thread.Sleep(5000);
            player.stats.isAlive = true;
            PlayerRespawn(player);
            yield return null;
        }
        public void PlayerRespawn(Player player = null)
        {
            if(player == null) 
            {
                player = new Player();
                MainManager.currentCreatureIDCount++;
                player.id = MainManager.currentCreatureIDCount;
            }
            if (player.isMainPlayer)
            {
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

            } else 
            {
                FakePlayerSpawn(player);
            }
            if (!MainManager.playerList.Any(c => c.id == player.id))
            {
                MainManager.playerList.Add(player);
            }
        }

        public void FakePlayerSpawn(Player player) 
        {
            GameObject fakePlayer = GameObject.Instantiate(fakePlayerPrefab);
            fakePlayerPrefab.GetComponent<PlayerData>().player = player;
            if (!MainManager.playerList.Any(c => c.id == player.id))
            {
                MainManager.playerList.Add(player);
            }
        }
    }
}