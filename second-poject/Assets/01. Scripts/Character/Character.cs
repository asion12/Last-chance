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

    public Elements_int characterResistElements = new Elements_int();
    public Elements_int characterWeakElements = new Elements_int();

    public Elements_int additionResistElements = new Elements_int();
    public Elements_int additionWeakElements = new Elements_int();

    public Elements totalResistElements = new Elements();
    public Elements totalWeakElements = new Elements();

    public bool isOverResist = false;
    public bool isOverWeak = false;

    public bool battleMode = false;
    public int max_carelessCounter = 0;
    public int carelessCounter = 0;
    public bool isCareless = false;

    public List<SO_Skill> skillList;
    public bool nowBuffing = false;
    public bool nowDebuffing = false;

    public bool isStunned = false;

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
        if (carelessCounter >= max_carelessCounter)
        {
            isCareless = true;
        }
        else
        {
            isCareless = false;
        }
    }

    public virtual void SetTotalStats()
    {
        totalStats.MAX_HP = characterStats.MAX_HP + buff_debuffStats.MAX_HP;
        totalStats.MAX_MP = characterStats.MAX_MP + buff_debuffStats.MAX_MP;
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

    private void SetToatalElelments(bool[] toResEl, bool[] toWckEl)
    {
        totalResistElements.SOLAR = toResEl[0];
        totalResistElements.LUMINOUS = toResEl[1];
        totalResistElements.IGNITION = toResEl[2];
        totalResistElements.HYDRO = toResEl[3];
        totalResistElements.BIOLOGY = toResEl[4];
        totalResistElements.METAL = toResEl[5];
        totalResistElements.CLAY = toResEl[6];

        totalWeakElements.SOLAR = toWckEl[0];
        totalWeakElements.LUMINOUS = toWckEl[1];
        totalWeakElements.IGNITION = toWckEl[2];
        totalWeakElements.HYDRO = toWckEl[3];
        totalWeakElements.BIOLOGY = toWckEl[4];
        totalWeakElements.METAL = toWckEl[5];
        totalWeakElements.CLAY = toWckEl[6];
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
        SetToatalElelments(toResEl, toWckEl);
    }

    public virtual void ResetBattleStatus()
    {
        buff_debuffStats = new Stats();
        additionResistElements = new Elements_int();
        additionWeakElements = new Elements_int();
        carelessCounter = 0;
    }
    // public void GetStat(int statID)
    // {
    //     switch (statID)
    //     {
    //     }
    // }
}
