using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Linq;
[CreateAssetMenu(fileName ="New Item Database",menuName = "Inventory/DB")]
public class ItemDBObj : ScriptableObject
{
    public ItemObj[] itemObjs;

    public void OnValidate()
    {
        for(int i = 0; i < itemObjs.Length; i++)
        {
            itemObjs[i].itemData.item_id = i;
        }
    }
}
