using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PowerSystem
{
    private int Power;
    [HideInInspector]
    public int CurrentPower = 0;

    public void PowerBoost(int PowerBoostData)
    {
        CurrentPower = Mathf.Clamp(CurrentPower + PowerBoostData, Power, int.MaxValue);
        Powersynchronization();
    }

    public void SetPower(int PowerData)
    {
        Power = Mathf.Clamp(PowerData, 0, int.MaxValue);
        Powersynchronization();
    }

    public void PowerMinus(int PowerMinusData)
    {
        Power = Mathf.Clamp(CurrentPower - PowerMinusData, 0, int.MaxValue);
        Powersynchronization();
    }

    public void Powersynchronization()
    {
        CurrentPower = Power;
    }
}
