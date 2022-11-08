using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_Item", menuName = "ScriptableObjects/Item", order = 0)]
public class SO_Item : ScriptableObject
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