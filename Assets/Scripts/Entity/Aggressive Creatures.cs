using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AggressiveCreatures : MonoBehaviour
{
    //Wandering
    private bool wandering;
    private Vector3 wanderingPos;
    private float wanderingTick;

    private bool aggressive;
    private GameObject target;

    public NavMeshAgent pathFinding;

    void Start()
    {
        aggressive = false;
        wandering = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (aggressive)
        {
            Attack();
        }
        else
        {
            Wander();
        }


    }

    void Attack()
    {

        //Deagro
        if (!MainManager.isPlayerAlive && aggressive)
        {
            aggressive = false;
            wandering = true;
            Wander();
        }
        if (gameObject.transform.position.x - target.transform.position.x > 25 || gameObject.transform.position.y - target.transform.position.y > 25 || gameObject.transform.position.z - target.transform.position.z > 25)
        {
            aggressive = false;
            wandering = true;
        } else
        {
            pathFinding.SetDestination(target.transform.position);
        }
    }

    void Wander()
    {
        wanderingTick += 1 * Time.deltaTime;
        if (wandering && wanderingTick >= 5)
        {
            wanderingPos = new Vector3(Random.Range(gameObject.transform.position.x - 10f, gameObject.transform.position.x + 10f), gameObject.transform.position.y, Random.Range(gameObject.transform.position.z - 10f, gameObject.transform.position.z + 10f));
            pathFinding.SetDestination(wanderingPos);
            wanderingTick = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            aggressive = true;
            target = other.gameObject;
            wandering = false;
        }
    }
}
