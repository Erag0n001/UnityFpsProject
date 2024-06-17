using System;
using UnityEngine.AI;
using UnityEngine;

namespace Shared
{
    [Serializable]
    public class Creature
    {
        public int id;
        public string name;
        public CreatureStats stats = new CreatureStats();
    }

    [Serializable]
    public class CreatureStats
    {
        public float hitPoint;
        public float damage;
        public float agroRange;
        public int enemyQuality;
        public float minSize;
        public float maxSize;
        public float baseSpeed;
        public float sprintingSpeed;
        public AIType aIType;
        public enum AIType { PassiveFlee, Passive, Neutral, Aggressive };
        public bool hit;
        public bool isAttackerPlayer;
        public NavMeshAgent pathFinding = null;
        public GameObject attacker = null;
        
        public bool wandering;
        public Vector3 wanderingPos;
        public float wanderingTick;

        public bool fleeing;

        public bool aggressive;
        public SphereCollider agroRangeCollider = null;

        public float immunityFrames;
    }
    [Serializable]
    public class CreatureBase
    {
        public int id;
        public string name;
        public CreatureStatsBase stats = new CreatureStatsBase();
    }
    [Serializable]
    public class CreatureStatsBase
    {
        public float hitPoint;
        public float damage;
        public float agroRange;
        public int enemyQuality;
        public float minSize;
        public float maxSize;
        public float baseSpeed;
        public float sprintingSpeed;
        public AIType aIType;
        public enum AIType { PassiveFlee, Passive, Neutral, Aggressive };
    }
}