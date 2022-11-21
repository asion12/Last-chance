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
    // 3: 
    // Start is called before the first frame update

    public Character targetEnemy;
    public Character targetCharacter;
    public Character player;
    public bool isTurnUsed = false;
    public bool isCarelessTurnUsed = false;

    void Start()
    {
    }

    private void Update()
    {
        //Debug.Log("Now Turn is " + nowTurnID.ToString());
        if (targetEnemy != null)
        {
            //Debug.Log(targetEnemy.nowHP);
            //Debug.Log(targetEnemy.nowMP);
        }
        if (nowTurnID != 0)
        {
            if (targetEnemy.nowHP <= 0)
                BattleEnd(true);
            else if (player.nowHP <= 0)
                BattleEnd(false);
        }
    }

    public void TurnChange()
    {
        if (nowTurnID == 1)
        {
            Debug.Log("Turn Change To Enemy");
            nowTurnID = 2;
        }
        else if (nowTurnID == 2)
        {
            nowTurnID = 1;
            Debug.Log("Turn Change To Player");
        }
        else
        {
            Debug.Log("NoOnesTurn!");
        }
        //Debug.Log("Turn Change!");
    }

    public void BattleStart(bool isPlayerStart, bool isVictimCareless, GameObject detactedEnemy)
    {
        Debug.Log("BattleStart!");
        SetEnemy(detactedEnemy.GetComponent<Character>());
        targetEnemy.battleMode = true;
        player.battleMode = true;
        player.GetComponent<Player>().CameraRotateToTarget(targetEnemy.transform.gameObject);
        if (isPlayerStart)
        {
            nowTurnID = 1;
            if (isVictimCareless)
                targetEnemy.carelessCounter = targetEnemy.max_carelessCounter;
        }
        else
        {
            nowTurnID = 2;
        }
    }

    public IEnumerator BattleRun(bool isPlayerRun)
    {
        nowTurnID = 0;
        targetEnemy.battleMode = false;
        player.battleMode = false;
        Debug.Log(player.battleMode);
        ResetCharactersBattleStatus();
        if (isPlayerRun)
        {
            Debug.Log("Enemy Stunned!");
            targetEnemy.isStunned = true;
            yield return new WaitForSeconds(5);
            targetEnemy.isStunned = false;
        }
        else
        {
            Debug.Log("Player Stunned!");
            player.isStunned = true;
            yield return new WaitForSeconds(5);
            player.isStunned = false;
        }
    }

    private void ResetCharactersBattleStatus()
    {
        player.ResetBattleStatus();
        targetEnemy.ResetBattleStatus();
    }

    public void BattleEnd(bool isPlayerWin)
    {
        nowTurnID = 0; // reset turn
        player.battleMode = false;
        targetEnemy.battleMode = false;

        if (isPlayerWin)
        {
            Debug.Log("Player Win");
            Destroy(targetEnemy.gameObject);
            targetEnemy = null;
        }
        else
        {
            Debug.Log("Enemy Win!");
        }
    }

    public void SetEnemy(Character setEnemy)
    {
        targetEnemy = setEnemy;
    }

    public void SetCharacter(Character setEnemy)
    {
        targetCharacter = setEnemy;
    }
    public void CastSkill(Character skillCaster, Character skillVictim, SO_Skill castSkill)
    {
        if (skillCaster.nowMP < castSkill.needMp)
        {
        }
        else
        {
            skillCaster.nowMP -= castSkill.needMp;

            bool isCritical = false;
            bool isAdvantage = false;
            bool isRejct = false;
            bool isGuard = false;
            bool isMiss = false;
            bool isDeception = false;
            int overDealing = 0;
            Debug.Log("Casted!");

            if (!skillVictim.isCareless)
            {
                isTurnUsed = true;
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
                castSkill.accuarityPer -= skillVictim.totalStats.DEX;
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
                    if (CheckElement(skillVictim.totalResistElements, castSkill.skillElements))
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

                        if (CheckElement(skillVictim.totalWeakElements, castSkill.skillElements))
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
            }
            else
            {
                isCarelessTurnUsed = true;
                isCritical = true;
                isAdvantage = true;
                overDealing = skillVictim.carelessCounter - skillVictim.max_carelessCounter;
                Debug.Log("Careless!");

                skillVictim.carelessCounter = 0;
                skillVictim.isCareless = false;
            }

            float increaseDamage = 0f;
            float decreaseDamage = 0f;

            if (castSkill.categorPyhysics)
            {
                increaseDamage = skillCaster.totalStats.STR;
                decreaseDamage = skillVictim.totalStats.FIR;
            }
            else if (castSkill.categoryChemistry)
            {
                Debug.Log("Chemistry");
                increaseDamage = skillCaster.totalStats.INT;
                decreaseDamage = skillVictim.totalStats.WIS;
            }

            float checkDamage = castSkill.skillDamage * (1 + (increaseDamage / 100));
            Debug.Log("Check Damage is " + checkDamage);

            float finalSkillDamage =
            (castSkill.skillDamage * (1 + (increaseDamage / 100)) * (1 + overDealing + Convert.ToInt32(isAdvantage))
            * (1 - (decreaseDamage * Convert.ToInt32(!isCritical) / 100))) * Convert.ToInt32(!isRejct);

            Debug.Log("Final Skill Damage is " + finalSkillDamage.ToString());
            skillVictim.nowHP -= Convert.ToInt32(finalSkillDamage);
            Debug.Log("Now Victim Hp : " + skillVictim.nowHP.ToString());


            ResetSkillNurmical(castSkill);
            CheckTurnChange(skillVictim);
        }
    }

    public void CastOverClock(Character overClockCaster, Character overClockVictim, List<SO_Skill> overClockSkillList)
    {
        Elements_int tempPlayerAddWckEl = overClockCaster.additionWeakElements;
        for (int i = 0; i < overClockSkillList.Count; i++)
        {
            SetOverClock(overClockSkillList[i]);
            if (overClockCaster.isOverWeak)
            {
                OverClockEnd(overClockCaster, tempPlayerAddWckEl);
                break;
            }
        }
    }

    public void SetOverClock(SO_Skill overClockSkill)
    {
        int[] nowPlayerAddWckElTemp = Element_intArrReturn(player.additionWeakElements);
        bool[] nowSkillElTemp = ElementArrReturn(overClockSkill.skillElements);
        for (int i = 0; i < 7; i++)
        {
            if (nowSkillElTemp[i])
            {
                nowPlayerAddWckElTemp[i]++;
                overClockSkill.isOverClockSet = true;
            }
        }
    }

    private void OverClockEnd(Character resetCharacter, Elements_int resetElements_int)
    {
        Debug.Log("OverClockEnded!");
        resetCharacter.additionWeakElements = resetElements_int;
        for (int i = 0; i < resetCharacter.skillList.Count; i++)
        {
            if (resetCharacter.skillList[i].isOverClockSet)
            {
                resetCharacter.skillList[i].isOverClockSet = false;
            }
        }
    }

    private void CheckTurnChange(Character checkCharacter)
    {
        if (checkCharacter.carelessCounter >= checkCharacter.max_carelessCounter)
        {
            Debug.Log("OneMore");
        }
        else
        {
            if (!isTurnUsed)
            {
                Debug.Log("One More");
            }
            else
            {
                Debug.Log("TurnChange");
                isTurnUsed = false;
                isCarelessTurnUsed = false;
                TurnChange();
            }
        }
    }

    private void ResetSkillNurmical(SO_Skill resetSkill)
    {
        resetSkill.accuarityPer = 100;
        resetSkill.casterCriticalPer = 0;
        resetSkill.victimDeceptionPer = 0;
    }

    private bool[] ElementArrReturn(Elements el)
    {
        bool[] elementArr = new bool[7];
        elementArr[0] = el.SOLAR;
        elementArr[1] = el.LUMINOUS;
        elementArr[2] = el.IGNITION;
        elementArr[3] = el.HYDRO;
        elementArr[4] = el.BIOLOGY;
        elementArr[5] = el.METAL;
        elementArr[6] = el.CLAY;

        return elementArr;
    }

    private int[] Element_intArrReturn(Elements_int el)
    {
        int[] elementArr = new int[7];
        elementArr[0] = el.SOLAR;
        elementArr[1] = el.LUMINOUS;
        elementArr[2] = el.IGNITION;
        elementArr[3] = el.HYDRO;
        elementArr[4] = el.BIOLOGY;
        elementArr[5] = el.METAL;
        elementArr[6] = el.CLAY;

        return elementArr;
    }

    private bool CheckElement(Elements characterElements, Elements skillElements)
    {

        int count = 0;

        bool[] characterEl = ElementArrReturn(characterElements);
        bool[] skillEl = ElementArrReturn(skillElements);

        for (int i = 0; i < 7; i++)
        {
            if (characterEl[i] && skillEl[i])
                count++;
        }

        // for (int i = 0; i < 7; i++)
        // {
        //     switch (i)
        //     {
        //         case 0:
        //             if (characterElements.SOLAR && skillElements.SOLAR)
        //                 count++;
        //             break;
        //         case 1:
        //             if (characterElements.LUMINOUS && skillElements.LUMINOUS)
        //                 count++;
        //             break;
        //         case 2:
        //             if (characterElements.IGNITION && skillElements.IGNITION)
        //                 count++;
        //             break;
        //         case 3:
        //             if (characterElements.HYDRO && skillElements.HYDRO)
        //                 count++;
        //             break;
        //         case 4:
        //             if (characterElements.BIOLOGY && skillElements.BIOLOGY)
        //                 count++;
        //             break;
        //         case 5:
        //             if (characterElements.METAL && skillElements.METAL)
        //                 count++;
        //             break;
        //         case 6:
        //             if (characterElements.CLAY && skillElements.CLAY)
        //                 count++;
        //             break;
        //         default:
        //             break;
        //     }
        // }

        if (count > 0)
            return true;
        else
            return false;
    }
}