using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    [Serializable]
    public class Stats
    {
        public float maxHitpoint;
        public float hitPoint;
        public float damage;
        public float baseMovementSpeed;
        public float movementSpeed;
        public float maxStamina;
        public float stamina;
        public float baseJumpPower;
        public float jumpPower;
        [NonSerialized] public float immunityFrames;
    }
    public Stats stats;
    private StatTemplate.Stats enemyStats;
    private GameObject enemy;
    private GameObject deadPlayerPrebad;

    private Vector3 knockbackVector;
    private CharacterController controller;
    private StatTemplate statTemplate;
    private GameObject deadList;

    private void Awake()
    {
        deadPlayerPrebad = Resources.Load<GameObject>("Prefab/Entities/Player/DeadPlayer");
    }

    private void Start()
    {
        deadList = GameObject.Find("Dead");
    }
    // Update is called once per frame
    void Update()
    {
        StatsUpdate();
        IsPlayerDead();
    }
    private void OnTriggerStay(Collider other)
    {
        enemy = other.gameObject;
        DamageCheck();
    }

    void DamageCheck()
    {
        if (enemy.tag == "Entity" && stats.immunityFrames <= 0)
        {
            statTemplate = enemy.GetComponent<StatTemplate>();
            enemyStats = statTemplate.stats;
            if (enemyStats.damage != 0)
            {
                ThreadingMisc.Threader(delegate { this.AddHP(enemyStats.damage * -1); });
                SetImmunityFrames(0.1f);
            }
        }
    }
    
    void KillPlayer()
    {
        MainManager.latestPlayerDeadBody = GameObject.Instantiate(deadPlayerPrebad, gameObject.transform.position, Quaternion.identity, deadList.transform);
        MainManager.isPlayerAlive = false;
        GameObject.Destroy(gameObject);
        Camera camera = MainManager.latestPlayerDeadBody.transform.Find("PlayerDeadCamera").Find("Camera").gameObject.GetComponent<Camera>();
        camera.enabled = true;
    }

    public void StatsUpdate()
    {
        if (stats.immunityFrames > 0)
        {
            SetImmunityFrames(stats.immunityFrames - 1 * Time.deltaTime);
        }
    }

    public void IsPlayerDead()
    {
        if (stats.hitPoint <= 0)
        {
            KillPlayer();
        }
    }
    public void AddHP(float modifier) { stats.hitPoint += modifier; }
    public void AddStamina(float modifier){ stats.stamina += modifier; }
    public void SetMovemendSpeed(float modifier) { stats.movementSpeed = modifier; }
    public void SetImmunityFrames(float modifier) { stats.immunityFrames = modifier; }
}