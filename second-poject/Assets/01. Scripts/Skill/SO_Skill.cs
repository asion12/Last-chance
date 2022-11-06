using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class SO_Skill : ScriptableObject
{
    public string skillName;
    public Elements skillElements;
    public int skillDamage;
    public float needFOC;
    public float needMp;
    public float needHp;

    public float accuarityPer = 100;
    public float casterCriticalPer;
    public float victimDeceptionPer;
    public SideEffect sideEffect;
    // 0: NULL - Just Damag 
    // 1: SUN - Attack to Light
    // 2: MOON - Attack to Hack
    // 3: FIRE - Attack to Fire
    // 4: WATER - Attack to Water
    // 5: WOOD - Attack And Drain
    // 6: GOLD - Attack to Knife & Gun
    // 7: SAND - Attack to Durt & Sand
}
