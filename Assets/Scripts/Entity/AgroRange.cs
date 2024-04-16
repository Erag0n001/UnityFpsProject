using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AgroRange : MonoBehaviour
{
    CreatureAI creatureAI;
    public List<GameObject> enemyInRange;
    void Start()
    {
        creatureAI = gameObject.GetComponentInParent<CreatureAI>();
    }
    
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject != creatureAI.gameObject)
        {
            if(!enemyInRange.Contains(other.gameObject))
            {
                if(other.gameObject.CompareTag("Entity") || other.gameObject.CompareTag("Player"))
                {
                    enemyInRange.Add(other.gameObject);
                }   
            }
        }

    }

    void Update()
    {
        CheckAgro();
    }

    void CheckAgro()
    {
        if(enemyInRange.Count > 0)
        {
            enemyInRange.OrderBy(enemy => Vector3.Distance(transform.position, enemy.transform.position));
            creatureAI.SetAgro(enemyInRange[0]);
            enemyInRange.Clear();
        }
    }
}
