using Client;
using System;
using System.Numerics;
using UnityEngine;
namespace Shared
{
    [Serializable]
    public class Creature
    {
        public int instanceId;
        public string instanceName;
        public string uniqueName;
        public CreatureStats stats = new CreatureStats();
        [NonSerialized] public CreatureAI creatureAI;

        public bool receivedPacketMove;
        public bool receivedPacketDeath;
        public bool needsUpdating;
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
        [NonSerialized] public bool hit;
        [NonSerialized] public bool isAttackerPlayer;
        [NonSerialized] public GameObject attacker = null;
        
        public SerializableVector3 wanderingPos;
        [NonSerialized] public float wanderingTick;

        public SerializableVector3 currentPosition;
        public SerializableVector4 currentRotation;

        [NonSerialized] public Status status;
        public enum Status { Wandering, Attacking, Fleeing};

        [NonSerialized] public SphereCollider agroRangeCollider = null;

        [NonSerialized] public float immunityFrames;
    }
    [Serializable]
    public class CreatureBase
    {
        public string uniqueName;
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