using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal static class ThreadingInventory
{
    public static async Task<Item> CheckForExistingItemAsync(int itemID)
    {
        return await Task.Run(() =>
        {
            return MainManager.inventoryManager.CheckForExistingItem(itemID);
        });
    }
}
