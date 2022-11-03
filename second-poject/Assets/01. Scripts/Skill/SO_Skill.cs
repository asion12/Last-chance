using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class SO_Skill : ScriptableObject
{
    public float skillDamage;
    public float needFocus;
    public float needMp;
    public float needHp;
    public int ElementID;
    // 0: 무
    // 1: 일
    // 2: 월
    // 3: 화
    // 4. 수
    // 5: 목
    // 6: 금
    // 7: 토
}
