using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class SO_Skill : ScriptableObject
{
    public string skillName;
    public Elements skillElements;
    public float skillDamage;
    public float needFOC;
    public float needMp;
    public float needHp;

    public bool categorPyhysics;
    public bool categoryChemistry;

    public float accuarityPer = 100;
    public float casterCriticalPer;
    public float victimDeceptionPer;
    public SideEffect sideEffect;
}
