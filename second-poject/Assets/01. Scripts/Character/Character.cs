using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [System.Serializable]
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

        public Elements resistElement;
        public List<int> weakElement;

        public Stats(int Hp, int Mp, float Str, float Fir, float Int, float Wis, float Foc, float Dex, float Cha, List<int> _resistlement, List<int> _weakElement)
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
            resistElement = _resistlement;
            weakElement = _weakElement;
        }
    }

    public Stats characterStats = new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, new List<int>(), new List<int>());

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
