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

    private Elements_int characterResistElements = new Elements_int();
    private Elements_int characterWeakElements = new Elements_int();

    private Elements_int additionResistElements = new Elements_int();
    private Elements_int additionWeakElements = new Elements_int();

    public Elements totalResistElements = new Elements();
    public Elements totalWeakElements = new Elements();

    public bool isOverResist = false;
    public bool isOverWeak = false;

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

    private int[] Elements_IntArrReturn(Elements_int el)
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

    private bool[] ElementsArrReturn(Elements el)
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
        int[] chResEl = Elements_IntArrReturn(characterResistElements);
        int[] chWckEl = Elements_IntArrReturn(characterWeakElements);
        int[] adResEl = Elements_IntArrReturn(additionResistElements);
        int[] adWckEl = Elements_IntArrReturn(additionWeakElements);

        bool[] toResEl = ElementsArrReturn(totalResistElements);
        bool[] toWckEl = ElementsArrReturn(totalWeakElements);
        for (int i = 0; i < 7; i++)
        {
            if ((chResEl[i] + adResEl[i]) - (chWckEl[i] + adWckEl[i]) == 0)
            {
                toResEl[i] = false;
                toWckEl[i] = false;
            }
            else if ((chResEl[i] + adResEl[i]) - (chWckEl[i] + adWckEl[i]) > 0) //if Character Resist
            {
                toResEl[i] = true;
                toWckEl[i] = false;

                if ((chResEl[i] + adResEl[i]) - (chWckEl[i] + adWckEl[i]) > 1)
                    isOverResist = true;
                else
                    isOverResist = false;
            }
            else if ((chResEl[i] + adResEl[i]) - (chWckEl[i] + adWckEl[i]) < 0) //if Character Resist
            {
                toResEl[i] = false;
                toWckEl[i] = true;

                if ((chResEl[i] + adResEl[i]) - (chWckEl[i] + adWckEl[i]) < -1)
                    isOverWeak = true;
                else
                    isOverWeak = false;
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
