using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class SO_Skill : ScriptableObject
{
    public string skillName;
    public bool categorPyhysics;
    public bool categoryChemistry;
    public bool isCanUse = true;
    public Elements skillElements;
    public Elements setResistElements;
    public Elements setWeakElements;
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

    [SerializeField]
    public SkillDivision skillDivision = SkillDivision.ATTACK;

    public float accuarityPer = 100;
    public float casterCriticalPer;
    public float victimDeceptionPer;
    public SideEffect sideEffect;
}
