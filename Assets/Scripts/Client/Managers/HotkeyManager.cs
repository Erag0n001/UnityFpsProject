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

    [Header("Other UI")]
    [SerializeField] private GameObject canvas;
    private void Awake()
    {
        MainManager.hotkeyManager= this;
    }
    void Start()
    {
        keys = new KeyCode[] { KeyCode.Tab,KeyCode.LeftShift, KeyCode.LeftControl, KeyCode.Space, KeyCode.Mouse0};
    }

    void Update()
    {
        for (int i = 0; i < keys.Length; i++)
        {
            if (Input.GetKeyDown(keys[i]))
            {
                switch (keys[i])
                {
                    case KeyCode.Tab: MainManager.inventoryManager.InventoryUIShow(); break;
                    case KeyCode.LeftShift: MainManager.playerMovement.SprintingDown();  break;
                    case KeyCode.LeftControl: MainManager.playerMovement.CrouchingDown(); break;
                    case KeyCode.Space: MainManager.playerMovement.Jump(); break;
                    case KeyCode.Mouse0 : MainManager.playerHotbar.UseItem(); break;
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
            //Hotbar stuff
            if(Input.mouseScrollDelta.y > 0)
            {
                if(MainManager.playerHotbar.currentSlot == 10)
                {
                    MainManager.playerHotbar.currentSlot = 0;
                } 
                else 
                {
                    MainManager.playerHotbar.currentSlot +=1;
                }
            } 
            else if(Input.mouseScrollDelta.y < 0)
            {
                if(MainManager.playerHotbar.currentSlot == 0)
                {
                    MainManager.playerHotbar.currentSlot = 10;
                } 
                else 
                {
                    MainManager.playerHotbar.currentSlot -=1;
                }
            } 
        }
    }
}
