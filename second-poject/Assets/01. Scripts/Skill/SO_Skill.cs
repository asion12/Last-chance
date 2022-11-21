using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill", order = 1)]
public class SO_Skill : ScriptableObject
{
    public bool isCanUse = true;
    public string skillName;
    public Elements skillElements;
    public float skillDamage;
    public float needFOC;
    public int needMp;
    public int needHp;

    public bool categorPyhysics;
    public bool categoryChemistry;

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
