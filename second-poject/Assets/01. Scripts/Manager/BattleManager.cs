using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void TurnChange(int i, int m, int n)
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
        Debug.Log("Casted!");
        void AddCarelessCounter(Character character)
        {
            character.carelessCounter++;
        }

        bool PercentageCheck(float percnetage)
        {
            float accuarityRoll = Random.Range(0f, 100f);
            if (accuarityRoll < percnetage)
                return true;
            else
                return false;
        }

        // SkillCaster FOC Check
        if (skillCaster.characterStats.FOC > castSkill.needFOC)
        {
            castSkill.casterCriticalPer = skillCaster.characterStats.FOC - castSkill.needFOC;
        }
        else if (skillCaster.characterStats.FOC < castSkill.needFOC)
        {
            castSkill.accuarityPer -= castSkill.needFOC - skillCaster.characterStats.FOC;
        }
        else
        {
        }

        // SkillVicTim DEX Check
        castSkill.accuarityPer -= skillCaster.characterStats.DEX;
        if (castSkill.accuarityPer < 0)
        {
            castSkill.victimDeceptionPer += -1 * castSkill.accuarityPer;
            castSkill.accuarityPer = 0;
        }

        // Skill Deception Percentage Add to CHA
        castSkill.victimDeceptionPer += skillVictim.characterStats.CHA;

        // Skill Hit Check
        if (PercentageCheck(castSkill.accuarityPer))
        {
            if (CheckElement(skillVictim.resistElements, castSkill.skillElements))
            {
                Debug.Log("Guard!");
                if (PercentageCheck(castSkill.victimDeceptionPer))
                {
                    Debug.Log("Deception!");
                }
            }
            else
            {
                if (PercentageCheck(castSkill.casterCriticalPer))
                {
                    Debug.Log("Critical!");
                }

                if (CheckElement(skillVictim.weakElements, castSkill.skillElements))
                {
                    Debug.Log("Additive!");
                }

                Debug.Log("Hit!");
            }
        }
        else
        {
            Debug.Log("Miss!");

            if (PercentageCheck(castSkill.victimDeceptionPer))
            {
                Debug.Log("Deception!");
            }
        }
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