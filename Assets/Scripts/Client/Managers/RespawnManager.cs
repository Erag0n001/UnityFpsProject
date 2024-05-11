using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class RespawnManager : MonoBehaviour
{
    [Serialize]private List <GameObject> creatureList = new List<GameObject>();
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
        
        List<GameObject> aggressiveList = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures/Aggressive").ToList();
        List<GameObject> neutralList = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures/Neutral").ToList();
        List<GameObject> passiveList = Resources.LoadAll<GameObject>("Prefab/Entities/Creatures/Passive").ToList();
        creatureList.AddRange(aggressiveList);
        creatureList.AddRange(neutralList);
        creatureList.AddRange(passiveList);
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
        if (entityRespawnCooldown >= 1)
        {
            entityRespawnCooldown = 0;
            EntityRespawn();
        }
        if (!MainManager.isPlayerAlive)
        {
            playerRespawnCooldown += 1 * Time.deltaTime;
            if(playerRespawnCooldown >= 5)
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
        selectedCreature = creatureList[selectedNumber];
        selectedCreature = Object.Instantiate(selectedCreature, selectedPad.transform.position, Quaternion.identity, aliveList.transform);
        EntitySizeRandomizer(selectedCreature);
    }
    
    void EntitySizeRandomizer( GameObject selectedCreature)
    {
        CreatureAI creatureAI;
        creatureAI = selectedCreature.GetComponent<CreatureAI>();
        float sizeMultiplier = Random.Range(creatureAI.stats.minSize,creatureAI.stats.maxSize);
        CreatureAI statTemplate = selectedCreature.GetComponent<CreatureAI>();
        statTemplate.stats.hitPoint *= sizeMultiplier;
        statTemplate.stats.damage *= sizeMultiplier;
        if(creatureAI.stats.aIType == CreatureAI.CreatureStat.AIType.Aggressive)
        {
            statTemplate.stats.agroRange *= sizeMultiplier;
            selectedCreature.GetComponentInChildren<SphereCollider>().radius = statTemplate.stats.agroRange;
        }
        selectedCreature.GetComponent<NavMeshAgent>().speed *= sizeMultiplier;
        selectedCreature.transform.localScale *= sizeMultiplier;
    }
    void playerRespawn()
    {
        MainManager.alivePlayer = Object.Instantiate(playerPrefab,playerRespawnPads[Random.Range(0, playerRespawnPads.Length)].transform.position,Quaternion.identity, aliveList.transform);
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
