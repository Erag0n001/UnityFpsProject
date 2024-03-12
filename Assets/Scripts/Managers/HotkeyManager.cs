using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HotkeyManager : MonoBehaviour
{
    [Header("Hotkey Variables")]
    public KeyCode[] keys;

    [Header("Player Input")]
    [SerializeField] private ActiveEquipement activeEquipement;

    [Header("Inventory")]
    [SerializeField] private GameObject inventoryPrefab;
    private GameObject inventoryObject;
    bool inventoryBool;

    [Header("Other UI")]
    [SerializeField] private GameObject canvas;
    private void Awake()
    {
        MainManager.hotkeyManager= this;
    }
    void Start()
    {
        keys = new KeyCode[] { KeyCode.Tab,KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.Space, KeyCode.Mouse0};
        inventoryBool = true;
    }

    void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                switch (keys[i])
                {
                    case KeyCode.Tab: InventoryCheck(); break;
                    case KeyCode.LeftShift: MainManager.playerMovement.SprintingDown();  break;
                    case KeyCode.LeftControl: MainManager.playerMovement.CrouchingDown(); break;
                    case KeyCode.Space: MainManager.playerMovement.Jump(); break;
                }
            }
            if (Input.GetKeyUp(keys[i]))
            {
                switch (keys[i])
                {
                    case KeyCode.LeftShift: MainManager.playerMovement.SprintingUp(); break;
                    case KeyCode.LeftControl: MainManager.playerMovement.CrouchingUp(); break;
                }
            }
        }
    }

    void InventoryCheck()
    {
        if (inventoryBool)
        {
            inventoryObject = GameObject.Instantiate(inventoryPrefab);
            inventoryObject.transform.SetParent(canvas.transform, false);
            //inventoryObject.transform.GetComponent<"RectTransform"> = new Vector2(-400, -130);
            inventoryBool = false;
        }
        else
        {
            GameObject.Destroy(inventoryObject);
            inventoryBool = true;
        }
    }
}
