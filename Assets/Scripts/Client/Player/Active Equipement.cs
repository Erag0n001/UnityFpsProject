using UnityEngine;
namespace Client
{
    public class ActiveEquipement : MonoBehaviour
    {
        public Transform[] activeEquipementList;
        public GameObject[] equippedEquipementList;
        private KeyCode[] keys;
        void Start()
        {
            GameObject equipement = GameObject.Find("ActiveEquipement");
            activeEquipementList = equipement.GetComponentsInChildren<Transform>(true);
            int i = 0;
            foreach (Transform child in equipement.transform)
            {
                activeEquipementList[i] = child;
                i += 1;
            }
            equippedEquipementList = new GameObject[10];
            keys = new KeyCode[] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0 };
            equippedEquipementList[0] = activeEquipementList[0].transform.gameObject;

        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < keys.Length; i++)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    Unequip();
                    if (equippedEquipementList[i])
                    {
                        equippedEquipementList[i].SetActive(true);
                    }
                }
            }
        }

        void Unequip()
        {
            for (int i = 0; i < 10; i++)
            {
                if (equippedEquipementList[i])
                {
                    equippedEquipementList[i].SetActive(false);
                }
            }
        }
    }
}