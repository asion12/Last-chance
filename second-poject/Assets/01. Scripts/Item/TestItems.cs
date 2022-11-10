using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItems : MonoBehaviour
{
    public InventoryObj equipObj;
    public InventoryObj inventoryObj;
    public ItemDBObj itemDBObj;

    public void ClearInventory()
    {
        inventoryObj.Clear();   
    }
    public void AddnewItem()
    {
        if (itemDBObj.itemObjs.Length > 0)
        {
            ItemObj newItemObject = itemDBObj.itemObjs[Random.Range(0, itemDBObj.itemObjs.Length)];
            Item newItem = new Item(newItemObject);
            inventoryObj.AddItem(newItem, 1);
            Debug.Log("a?");

        }
    }

}
