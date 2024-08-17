using System;
[Serializable]
public class Player 
{
    int id;
    string name;
    PlayerStats stats;
}
[Serializable]
public class PlayerStats
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