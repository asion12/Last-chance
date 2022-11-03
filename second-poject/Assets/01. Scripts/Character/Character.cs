using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public struct Stats
    {
        public int HP;
        public int MP;
        public float STR;
        public float FIR;
        public float INT;
        public float WIS;
        public float FOC;
        public float DEX;
        public float CHA;

        public Stats(int Hp, int Mp, float Str, float Fir, float Int, float Wis, float Foc, float Dex, float Cha)
        {
            HP = Hp;
            MP = Mp;
            STR = Str;
            FIR = Fir;
            INT = Int;
            WIS = Wis;
            FOC = Foc;
            DEX = Dex;
            CHA = Cha;
        }
    }

    public Stats characterStats = new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0);

    private BattleManager battleManager;
    void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }
    public void GetStat(int statID)
    {
        switch (statID)
        {
            case 0:
                break;
            default:
                break;
        }
    }
}
