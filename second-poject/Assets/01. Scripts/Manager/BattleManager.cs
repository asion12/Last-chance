using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

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
    public NavMeshAgent monster;
    private int BonusExpScale = 0;
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
        CheckCharactersHp();
    }

    private void CheckCharactersHp()
    {
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
            uIManager.SetBattleUIInactive();
            Debug.Log("Turn Change To Enemy");
            nowTurnID = 2;
        }
        else if (nowTurnID == 2)
        {
            uIManager.SetBattleUIActive();
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
        // if now Battlemode
        if (nowTurnID != 0)
        {
            Debug.LogWarning("Now is Battle Mode But Trying Battle Start!");
        }
        else
        {
            SetEnemy(detactedEnemy.GetComponent<Character>());
            targetEnemy.isBattleMode = true;
            player.isBattleMode = true;
            Debug.Log("BattleStartFX!");
            uIManager.FX_BattleStart();
            Debug.Log("SkillUIOnFX!");
            uIManager.SetBattleUIActive();
            player.CameraRotateToTarget(targetEnemy.transform.gameObject);
            if (isPlayerStart)
            {
                //monster.speed = 0;
                nowTurnID = 1;
                if (isVictimCareless)
                    targetEnemy.carelessCounter = targetEnemy.max_carelessCounter;
            }
            else
            {
                nowTurnID = 2;
            }
        }
    }

    public void BattleEnd(bool isPlayerWin)
    {
        ResetBattleSetting();
        if (isPlayerWin)
        {
            float randomValue = UnityEngine.Random.Range(0.8f, 1.2f);
            player.EXP += (int)(20 * (1 + (player.GetLevelScale_forBattle(player.Level - targetEnemy.Level) * 2)) * randomValue * (1 + Mathf.Log(BonusExpScale + 1, 2)));
            BonusExpScale = 0;
            Debug.Log("Player Win");
            Debug.Log("Print UI What Player Get");
            uIManager.SetBattleUIInactive();
            Destroy(targetEnemy.gameObject);
            targetEnemy = null;
            //monster.speed = 3.5f;
        }
        else
        {
            BonusExpScale = 0;
            Debug.Log("Enemy Win!");
            Debug.Log("GameOverFX");
            Debug.Log("DropPlayerInventory");
            Debug.Log("GameRestart");
            uIManager.SetBattleUIInactive();
            GameManager.instance.ResetDungeon();
            //monster.speed = 3.5f;
        }
    }

    public IEnumerator BattleRun(bool isPlayerRun)
    {
        ResetBattleSetting();
        if (isPlayerRun)
        {
            uIManager.SetBattleUIInactive();
            Debug.Log("Enemy Stunned!");
            yield return new WaitForSeconds(5);
        }
        else
        {
            Debug.Log("Player Stunned!");
            yield return new WaitForSeconds(5);
        }
    }

    private void ResetBattleSetting()
    {
        nowTurnID = 0;
        targetEnemy.isBattleMode = false;
        player.isBattleMode = false;
        // Debug.Log(player.isBattleMode);
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
        if (skillCaster.nowMP < castSkill.needMp)
        {
        }
        else
        {
            skillCaster.nowMP -= castSkill.needMp;

            bool isCritical = false;
            bool isAdvantage = false;
            bool isReject = false;
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
                castSkill.accuarityPer += Mathf.Log(skillCaster.totalStats.FOC, 2) * 5;
                castSkill.casterCriticalPer += Mathf.Log(skillCaster.totalStats.FOC, 2) * 5;
                if (skillCaster.nowCP > castSkill.needCP)
                {
                    Debug.Log("Cp Over!!");
                    castSkill.accuarityPer += (skillCaster.nowCP - castSkill.needCP) * Mathf.Log(skillCaster.totalStats.FOC, 2);
                }
                else if (skillCaster.nowCP < castSkill.needCP)
                {
                    Debug.Log("Cp Min!!");
                    castSkill.accuarityPer -= (castSkill.needCP - skillCaster.nowCP) / Mathf.Log(skillCaster.totalStats.FOC, 2);
                }
                else
                {
                    Debug.Log("Cp fits");
                }

                Debug.Log("Skill ACC is " + castSkill.accuarityPer.ToString());

                // SkillVicTim DEX Check
                castSkill.accuarityPer = castSkill.accuarityPer
                - ((castSkill.accuarityPer * (Mathf.Log(skillVictim.totalStats.DEX, 2) - 1) / Mathf.Log(skillVictim.totalStats.DEX, 2))
                * (Mathf.Log(skillVictim.totalStats.CHA, 2) - 1 + (skillVictim.nowCP / skillVictim.maxCP))
                / Mathf.Log(skillVictim.totalStats.CHA, 2));

                Debug.Log("DexVal = " + Mathf.Log(skillVictim.totalStats.DEX, 2).ToString());
                Debug.Log("VicNowCP = " + skillVictim.nowCP + "VicMaxCP = " + skillVictim.maxCP + " CPVal = " + ((float)((float)skillVictim.nowCP / (float)skillVictim.maxCP)).ToString());
                Debug.Log("After Skill_1 ACC is " + castSkill.accuarityPer.ToString());

                castSkill.accuarityPer -= skillVictim.totalStats.DEX;

                Debug.Log("After Skill_2 ACC is " + castSkill.accuarityPer.ToString());

                castSkill.victimDeceptionPer += Mathf.Log(skillVictim.totalStats.CHA, 2) * 10;


                if (castSkill.accuarityPer > 100)
                {
                    castSkill.casterCriticalPer += castSkill.accuarityPer - 100;
                }
                else if (castSkill.accuarityPer < 0)
                {
                    castSkill.victimDeceptionPer += -1 * castSkill.accuarityPer * Mathf.Log(skillVictim.totalStats.CHA, 2);
                    castSkill.accuarityPer = (float)0;
                }
                // // Skill Deception Percentage Add to CHA
                // castSkill.victimDeceptionPer *= Mathf.Log( 1 + skillVictim.totalStats.CHA, 2);
                // castSkill.victimDeceptionPer += skillVictim.totalStats.CHA;

                // Skill Hit Check
                if (PercentageCheck(castSkill.accuarityPer))
                {
                    if (CheckElement(skillVictim.totalResistElements, castSkill.skillElements))
                    {
                        Debug.Log("Guard!");
                        skillCaster.carelessCounter++;
                        isReject = true;
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
                    isMiss = true;
                    isReject = true;
                    skillCaster.carelessCounter++;
                    if (PercentageCheck(castSkill.victimDeceptionPer))
                    {
                        Debug.Log("Deception!");
                        isDeception = true;
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

            if (isDeception)
            {
                BonusExpScale++;
                Debug.Log("기만 성공! 경험치 배율 조정됨!");
            }

            float increaseDamage = 0f;
            float decreaseDamage = 0f;

            if (castSkill.categorPhysics)
            {
                increaseDamage = Mathf.Log(skillCaster.totalStats.STR, 2);
                decreaseDamage = Mathf.Log(skillVictim.totalStats.FIR, 2);
            }
            else if (castSkill.categoryChemistry)
            {
                Debug.Log("Chemistry");
                increaseDamage = Mathf.Log(skillCaster.totalStats.INT, 2);
                decreaseDamage = Mathf.Log(skillVictim.totalStats.WIS, 2);
            }

            float checkDamage = castSkill.skillDamage * increaseDamage;
            Debug.Log("Check Damage is " + checkDamage);

            float finalSkillDamage =
            (castSkill.skillDamage * increaseDamage * (1 + overDealing + Convert.ToInt32(isAdvantage))
            / (isCritical ? 1 : decreaseDamage)) * Convert.ToInt32(!isReject);

            Debug.Log("Final Skill Damage is " + finalSkillDamage.ToString());
            skillVictim.nowHP -= (int)finalSkillDamage;
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