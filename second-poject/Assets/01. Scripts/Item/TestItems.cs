using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItems : MonoBehaviour
{
    public InventoryObj equipObj;
    public InventoryObj inventoryObj;
    public SO_Item so_Item;

    public void ClearInventory()
    {
        inventoryObj.Clear();   
    }
    public void AddnewItem()
    {
        if (so_Item.itemObjs.Length > 0)
        {
            ItemObj newItemObject = so_Item.itemObjs[Random.Range(0, so_Item.itemObjs.Length - 1)];
            Item newItem = new Item(newItemObject);
            inventoryObj.AddItem(newItem, 1);
            Debug.Log("a?");

        }
    }

}
