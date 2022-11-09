using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public int nowTurnID = 0;
    // 0: Battle Mode off
    // 1: PlayerTurn
    // 2: EnemyTurn
    // Start is called before the first frame update

    public Character targetEnemy;
    public Character player;


    void Start()
    {
    }

    private void Update()
    {
    }

    public void TurnChange()
    {
        if (nowTurnID == 1)
        {
            nowTurnID = 2;
        }
        if (nowTurnID == 2)
        {
            nowTurnID = 1;
        }
        else
        {
            Debug.Log("NoOnesTurn!");
        }
    }

    public void BattleStart(bool isPlayerStart, GameObject detactedEnemy)
    {
        Debug.Log("BattleStart!");
        SetEnemy(detactedEnemy.GetComponent<Character>());
        targetEnemy.battleMode = true;
        player.battleMode = true;
        player.GetComponent<Player>().CameraRotateToTarget(targetEnemy.transform.gameObject);
        if (isPlayerStart)
        {
            nowTurnID = 1;
        }
        else
        {
            nowTurnID = 2;
        }
    }

    public void BattleEnd(bool isPlayerWin)
    {
        nowTurnID = 0; // reset turn
        targetEnemy = null; // reset targetEnemy
        if (isPlayerWin)
        {
            Debug.Log("Player Win!");
        }
        else
        {
            Debug.Log("Enemu Win!");
        }
    }

    public void SetEnemy(Character setEnemy)
    {
        targetEnemy = setEnemy;
    }

    public void CastSkill(Character skillCaster, Character skillVictim, SO_Skill castSkill)
    {
        bool isCritical = false;
        bool isAdvantage = false;
        bool isRejct = false;
        bool isGuard = false;
        bool isMiss = false;
        bool isDeception = false;
        Debug.Log("Casted!");
        void AddCarelessCounter(Character character)
        {
            character.carelessCounter++;
        }

        bool PercentageCheck(float percnetage)
        {
            float accuarityRoll = UnityEngine.Random.Range(0f, 100f);
            if (accuarityRoll < percnetage)
                return true;
            else
                return false;
        }

        // SkillCaster FOC Check
        if (skillCaster.totalStats.FOC > castSkill.needFOC)
        {
            castSkill.casterCriticalPer = skillCaster.totalStats.FOC - castSkill.needFOC;
        }
        else if (skillCaster.totalStats.FOC < castSkill.needFOC)
        {
            castSkill.accuarityPer -= castSkill.needFOC - skillCaster.totalStats.FOC;
        }
        else
        {
        }

        // SkillVicTim DEX Check
        castSkill.accuarityPer -= skillCaster.totalStats.DEX;
        if (castSkill.accuarityPer < 0)
        {
            castSkill.victimDeceptionPer += -1 * castSkill.accuarityPer;
            castSkill.accuarityPer = 0;
        }

        // Skill Deception Percentage Add to CHA
        castSkill.victimDeceptionPer += skillVictim.totalStats.CHA;

        // Skill Hit Check
        if (PercentageCheck(castSkill.accuarityPer))
        {
            if (CheckElement(skillVictim.resistElements, castSkill.skillElements))
            {
                Debug.Log("Guard!");
                skillCaster.carelessCounter++;
                isRejct = true;
                isGuard = true;
                if (PercentageCheck(castSkill.victimDeceptionPer))
                {
                    Debug.Log("Deception!");
                    isDeception = true;
                    skillCaster.carelessCounter++;
                }
            }
            else
            {
                if (PercentageCheck(castSkill.casterCriticalPer))
                {
                    Debug.Log("Critical!");
                    isCritical = true;
                    skillVictim.carelessCounter++;
                }

                if (CheckElement(skillVictim.weakElements, castSkill.skillElements))
                {
                    Debug.Log("Advantage!");
                    isAdvantage = true;
                    skillVictim.carelessCounter++;
                }

                Debug.Log("Hit!");
            }
        }
        else
        {
            Debug.Log("Miss!");
            skillCaster.carelessCounter++;
            if (PercentageCheck(castSkill.victimDeceptionPer))
            {
                Debug.Log("Deception!");
                skillCaster.carelessCounter++;
            }
        }

        float increaseDamage = 0f;
        float decreaseDamage = 0f;

        if (castSkill.categorPyhysics)
        {
            increaseDamage = skillCaster.totalStats.STR;
            decreaseDamage = skillVictim.totalStats.FIR;
        }
        else if (castSkill.categoryChemical)
        {
            Debug.Log("Chemical");
            increaseDamage = skillCaster.totalStats.INT;
            decreaseDamage = skillVictim.totalStats.WIS;
        }

        float DamageCheck = castSkill.skillDamage * (1 + (increaseDamage / 100));
        float finalSkillDamage = (castSkill.skillDamage * (1 + (increaseDamage / 100)) * (1 + Convert.ToInt32(isAdvantage)) * (1 - (decreaseDamage * Convert.ToInt32(!isCritical) / 100))) * Convert.ToInt32(!isRejct);
        Debug.Log("Damage Check is " + DamageCheck.ToString());
        Debug.Log("Final Skill Damage is " + finalSkillDamage.ToString());
        skillVictim.totalStats.HP
        -= Convert.ToInt32(finalSkillDamage);
        Debug.Log("Now Victim Hp : " + skillVictim.totalStats.HP.ToString());

        ResetSkillNurmical(castSkill);
    }

    private void ResetSkillNurmical(SO_Skill resetSkill)
    {
        resetSkill.accuarityPer = 100;
        resetSkill.casterCriticalPer = 0;
        resetSkill.victimDeceptionPer = 0;
    }

    private bool CheckElement(Elements characterElements, Elements skillElements)
    {
        bool CheckElementDetail(bool victimElement, bool skillElement)
        {
            if (victimElement == true && skillElement == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int count = 0;
        for (int i = 0; i < 7; i++)
        {
            switch (i)
            {
                case 0:
                    if (CheckElementDetail(characterElements.SOLAR, skillElements.SOLAR))
                        count++;
                    break;
                case 1:
                    if (CheckElementDetail(characterElements.LUMINOUS, skillElements.LUMINOUS))
                        count++;
                    break;
                case 2:
                    if (CheckElementDetail(characterElements.IGNITION, skillElements.IGNITION))
                        count++;
                    break;
                case 3:
                    if (CheckElementDetail(characterElements.HYDRO, skillElements.HYDRO))
                        count++;
                    break;
                case 4:
                    if (CheckElementDetail(characterElements.BIOLOGY, skillElements.BIOLOGY))
                        count++;
                    break;
                case 5:
                    if (CheckElementDetail(characterElements.METAL, skillElements.METAL))
                        count++;
                    break;
                case 6:
                    if (CheckElementDetail(characterElements.CLAY, skillElements.CLAY))
                        count++;
                    break;
                default:
                    break;
            }
        }

        if (count > 0)
            return true;
        else
            return false;
    }
}