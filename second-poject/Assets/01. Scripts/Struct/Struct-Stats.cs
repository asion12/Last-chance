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

    public Stats(int _HP, int _MP, float _STR, float _FIR, float _INT, float _WIS, float _FOC, float _DEX, float _CHA)
    {
        HP = _HP;
        MP = _MP;
        STR = _STR;
        FIR = _FIR;
        INT = _INT;
        WIS = _WIS;
        FOC = _FOC;
        DEX = _DEX;
        CHA = _CHA;
    }
}
