using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AgroRange : MonoBehaviour
{
    CreatureAI creatureAI;
    public List<GameObject> enemyInRange;
    void Start()
    {
        creatureAI = gameObject.GetComponentInParent<CreatureAI>();
    }
    
    private void OnTriggerExit(Collider other)
    {
        enemyInRange.Remove(other.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != creatureAI.gameObject)
        {
            if(!enemyInRange.Contains(other.gameObject))
            {
                if(other.gameObject.CompareTag("Entity") || other.gameObject.CompareTag("Player"))
                {
                    enemyInRange.Insert(0,other.gameObject);
                }   
            }
        }
    }

    void FixedUpdate()
    {
        CheckMembers();
        CheckAgro();
    }

    void CheckMembers()
    {
        foreach(GameObject enemy in enemyInRange.ToArray())
        {
            if(enemy == null)
            {
                enemyInRange.Remove(enemy);
            }
        }
    }

    void CheckAgro()
    {
        if(enemyInRange.Count > 0)
        {
            enemyInRange = enemyInRange.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position)).ToList();

            creatureAI.SetAgro(enemyInRange[0]);
        }
    }
}
