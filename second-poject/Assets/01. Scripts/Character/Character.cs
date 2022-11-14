using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public int nowHP;
    public int nowMP;

    public Stats characterStats = new Stats();
    public Stats buff_debuffStats = new Stats();
    public Stats totalStats = new Stats();

    public Elements characterResistElements = new Elements();
    public Elements characterWeakElements = new Elements();

    public Elements additionResistElements = new Elements();
    public Elements additionWeakElements = new Elements();

    public Elements totalResistElements = new Elements();
    public Elements totalWeakElements = new Elements();

    public bool battleMode = false;
    public int max_carelessCounter = 0;
    public int carelessCounter = 0;
    public List<SO_Skill> skillList;
    public bool nowBuffing = false;
    public bool nowDebuffing = false;

    public int Level = 1;
    public int Exp = 0;

    protected virtual void Start()
    {
        nowHP = characterStats.MAX_HP;
        nowMP = characterStats.MAX_MP;
    }

    protected virtual void Update()
    {
        SetElements();
        SetTotalStats();
    }

    public virtual void SetTotalStats()
    {
        totalStats.STR = characterStats.STR + buff_debuffStats.STR;
        totalStats.FIR = characterStats.FIR + buff_debuffStats.FIR;
        totalStats.INT = characterStats.INT + buff_debuffStats.INT;
        totalStats.WIS = characterStats.WIS + buff_debuffStats.WIS;
        totalStats.FOC = characterStats.FOC + buff_debuffStats.FOC;
        totalStats.DEX = characterStats.DEX + buff_debuffStats.DEX;
        totalStats.CHA = characterStats.CHA + buff_debuffStats.CHA;
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

    public virtual void SetElements()
    {
        bool setElementsDetail(bool element1, bool element2)
        {
            if (element1 == true && element2 == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        for (int i = 0; i < 7; i++)
        {
            bool[] chResEl = ElementArrReturn(characterResistElements);
            bool[] chWckEl = ElementArrReturn(characterWeakElements);
            bool[] adResEl = ElementArrReturn(additionResistElements);
            bool[] adEckEl = ElementArrReturn(additionWeakElements);

            switch (i)
            {
                case 0:
                    if (setElementsDetail(characterResistElements.SOLAR, additionWeakElements.SOLAR))
                    {
                        characterResistElements.SOLAR = false;
                        characterWeakElements.SOLAR = false;
                    }
                    break;
                case 1:
                    if (setElementsDetail(characterResistElements.LUMINOUS, characterWeakElements.LUMINOUS))
                    {
                        characterResistElements.LUMINOUS = false;
                        characterWeakElements.LUMINOUS = false;
                    }
                    break;
                case 2:
                    if (setElementsDetail(characterResistElements.IGNITION, characterWeakElements.IGNITION))
                    {
                        characterResistElements.IGNITION = false;
                        characterWeakElements.IGNITION = false;
                    }
                    break;
                case 3:
                    if (setElementsDetail(characterResistElements.HYDRO, characterWeakElements.HYDRO))
                    {
                        characterResistElements.HYDRO = false;
                        characterWeakElements.HYDRO = false;
                    }
                    break;
                case 4:
                    if (setElementsDetail(characterResistElements.BIOLOGY, characterWeakElements.BIOLOGY))
                    {
                        characterResistElements.BIOLOGY = false;
                        characterWeakElements.BIOLOGY = false;
                    }
                    break;
                case 5:
                    if (setElementsDetail(characterResistElements.METAL, characterWeakElements.METAL))
                    {
                        characterResistElements.METAL = false;
                        characterWeakElements.METAL = false;
                    }
                    break;
                case 6:
                    if (setElementsDetail(characterResistElements.CLAY, characterWeakElements.CLAY))
                    {
                        characterResistElements.CLAY = false;
                        characterWeakElements.CLAY = false;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    // public void GetStat(int statID)
    // {
    //     switch (statID)
    //     {
    //     }
    // }
}
