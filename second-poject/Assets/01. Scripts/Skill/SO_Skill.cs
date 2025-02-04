using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class SO_Skill : ScriptableObject
{
    public string skillName;
    public int buyCost = 0;
    public int playerHavingCount = 0;
    public bool isSell = false;
    public bool playerSkillSetted = false;
    public bool playerSkillOrdered = false;
    public bool categoryPhysics;
    public bool categoryChemistry;
    // [System.NonSerialized] public bool isCanUse = true;
    public Elements skillElements;
    public Elements setWeakElements;
    public Elements setResistElements;
    public float skillDamage;
    public float needCP;
    public int needMp;
    public int needHp;

    public enum SkillDivision
    {
        ATTACK,
        BUFF_DEBUFF,
        HEAL
    }

    public SkillDivision skillDivision = SkillDivision.ATTACK;

    [System.NonSerialized] public float accuarityPer = 100;
    [System.NonSerialized] public float casterCriticalPer;
    [System.NonSerialized] public float victimDeceptionPer;
    public SideEffect sideEffect;
}
