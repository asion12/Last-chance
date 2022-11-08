using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Stats characterStats = new Stats();
    public Elements resistElements = new Elements();
    public Elements weakElements = new Elements();
    public bool battleMode = false;
    public int carelessCounter = 0;
    public List<SO_Skill> skillList;

    public int Level = 1;
    public float Exp = 0;

    protected virtual void Update()
    {
        setElements();
    }

    public virtual void setElements()
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
                    if (setElementsDetail(resistElements.IRON, weakElements.IRON))
                    {
                        resistElements.IRON = false;
                        weakElements.IRON = false;
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
