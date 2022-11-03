using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SO_Item", menuName = "ScriptableObjects/Item", order = 0)]
public class SO_Item : ScriptableObject
{
    public int itemID;
    // 0: HP회복
    // 1: MP 회복
    // 2: 능력치 상승
    // 3: 열쇠
    public float itemPower;
    // 0 ~ 2: 회복, 상승의 정도
}