using Shared;
using System;
using UnityEngine;
[Serializable]
public class Player 
{
    public int id;
    public string name;
    public bool isMainPlayer;
    public PlayerStats stats;
    public Inventory playerInventory;

    public bool receivedPacketMove;
    public bool receivedPacketDeath;
    public bool needsUpdating;

    [NonSerialized] GameObject playerInstance;
    [NonSerialized] GameClient gameClient;
}
[Serializable]
public class PlayerStats
{
    public SerializableVector3 currentPosition = new SerializableVector3();
    public SerializableVector4 currentRotation = new SerializableVector4();

    public SerializableVector3 clientPosition = new SerializableVector3();

    public bool isAlive;
    public float maxHitpoint;
    public float hitPoint;
    public float damage;
    public float baseMovementSpeed;
    public float movementSpeed;
    public float maxStamina;
    public float stamina;
    public float baseJumpPower;
    public float jumpPower;
    public float immunityFrames;
}