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
    public Elements resistElements = new Elements();
    public Elements weakElements = new Elements();
    public bool battleMode = false;
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
            switch (i)
            {
                case 0:
                    if (setElementsDetail(resistElements.SOLAR, weakElements.SOLAR))
                    {
                        resistElements.SOLAR = false;
                        weakElements.SOLAR = false;
                    }
                    break;
                case 1:
                    if (setElementsDetail(resistElements.LUMINOUS, weakElements.LUMINOUS))
                    {
                        resistElements.LUMINOUS = false;
                        weakElements.LUMINOUS = false;
                    }
                    break;
                case 2:
                    if (setElementsDetail(resistElements.IGNITION, weakElements.IGNITION))
                    {
                        resistElements.IGNITION = false;
                        weakElements.IGNITION = false;
                    }
                    break;
                case 3:
                    if (setElementsDetail(resistElements.HYDRO, weakElements.HYDRO))
                    {
                        resistElements.HYDRO = false;
                        weakElements.HYDRO = false;
                    }
                    break;
                case 4:
                    if (setElementsDetail(resistElements.BIOLOGY, weakElements.BIOLOGY))
                    {
                        resistElements.BIOLOGY = false;
                        weakElements.BIOLOGY = false;
                    }
                    break;
                case 5:
                    if (setElementsDetail(resistElements.METAL, weakElements.METAL))
                    {
                        resistElements.METAL = false;
                        weakElements.METAL = false;
                    }
                    break;
                case 6:
                    if (setElementsDetail(resistElements.CLAY, weakElements.CLAY))
                    {
                        resistElements.CLAY = false;
                        weakElements.CLAY = false;
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
