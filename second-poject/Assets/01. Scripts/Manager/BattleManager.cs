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
            Debug.Log("Instance Set");
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
    public Player player;
    public bool isTurnUsed = false;
    public bool isCarelessTurnUsed = false;
    private UIManager uIManager = null;

    void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
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
        SetEnemy(detactedEnemy.GetComponent<Character>());
        targetEnemy.isBattleMode = true;
        player.isBattleMode = true;
        Debug.Log("BattleStartFX!");
        uIManager.FX_BattleStart();
        Debug.Log("SkillUIOnFX!");
        uIManager.SetBattleUIActive(true);
        player.CameraRotateToTarget(targetEnemy.transform.gameObject);
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

    public void BattleEnd(bool isPlayerWin)
    {
        ResetBattleSetting();
        if (isPlayerWin)
        {
            Debug.Log("Player Win");
            Debug.Log("Print UI What Player Get");
            Destroy(targetEnemy.gameObject);
            targetEnemy = null;
        }
        else
        {
            Debug.Log("Enemy Win!");
            Debug.Log("GameOverFX");
            Debug.Log("DropPlayerInventory");
            Debug.Log("GameRestart");
        }
    }

    public IEnumerator BattleRun(bool isPlayerRun)
    {
        ResetBattleSetting();
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

    private void ResetBattleSetting()
    {
        nowTurnID = 0;
        targetEnemy.isBattleMode = false;
        player.isBattleMode = false;
        Debug.Log(player.isBattleMode);
        player.ResetBattleStatus();
        targetEnemy.ResetBattleStatus();
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
        float CasterCP = (float)skillCaster.nowCP;
        float CasterCP_Percentage = CasterCP / (float)skillCaster.maxCP;
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
                if (CasterCP > castSkill.needCP)
                {
                    castSkill.casterCriticalPer = CasterCP - castSkill.needCP;
                }
                else if (CasterCP < castSkill.needCP)
                {
                    castSkill.accuarityPer -= castSkill.needCP - CasterCP;
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

    public void SetOverClock()
    {

    }

    public void CastOverClockSkills(Player overClockSkillCaster, Character overClockSkillVictim)
    {
        List<SO_Skill> casterSkillList = overClockSkillCaster.skillList;
        for (int i = 0; i < casterSkillList.Count; i++)
        {
            if (overClockSkillCaster.isSkillOverClockList[i])
            {
                CastSkill(overClockSkillCaster, overClockSkillVictim, casterSkillList[i]);
                if (overClockSkillCaster.isCareless)
                {
                    OverClockEnd(overClockSkillCaster);
                    break;
                }
            }
        }
    }

    private void OverClockEnd(Player resetCharacter)
    {
        Debug.Log("OverClockEnded!");
        resetCharacter.isSkillOverClockList = new bool[] { false, };
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