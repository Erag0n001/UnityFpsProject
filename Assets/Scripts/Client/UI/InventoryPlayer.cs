using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPlayer : Inventory
{
    private void Awake()
    {
        MainManager.playerInventory = this;
    }
    
    public override void InventoryUIShow()
    {
        if (inventoryBool)
        {
            inventoryObject = GameObject.Instantiate(inventoryPrefab);
            inventoryObject.transform.SetParent(canvas.transform, false);
            inventoryBool = false;
            MainManager.playerCameraControl.UnlockCursor();
            InventoryUpdate();
        }
        else
        {
            GameObject.Destroy(inventoryObject);
            MainManager.playerCameraControl.LockCursor();
            inventoryBool = true;
        }
    }

    public void PlayerInventoryUIShow()
    {
        if (inventoryBool)
        {
            inventoryObject = GameObject.Instantiate(inventoryPrefab);
            inventoryObject.transform.SetParent(canvas.transform, false);
            inventoryBool = false;
            MainManager.playerCameraControl.UnlockCursor();
            InventoryUpdate();
        }
        else
        {
            GameObject.Destroy(inventoryObject);
            MainManager.playerCameraControl.LockCursor();
            inventoryBool = true;
        }
    }
}
