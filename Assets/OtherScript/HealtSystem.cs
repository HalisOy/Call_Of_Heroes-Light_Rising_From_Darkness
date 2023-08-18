using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HealtSystem
{
    public int MaxHealt;
    [HideInInspector]
    public int Healt;

    public void Damage(int DamageData)
    {
        Healt = Mathf.Clamp(Healt - DamageData, 0, MaxHealt);

    }

    public void HealtBoost(int HealtBoostData)
    {
        Healt = Mathf.Clamp(Healt + HealtBoostData, 0, MaxHealt);
    }

    public void SetHealt(int HealtData)
    {
        Healt = Mathf.Clamp(HealtData, 0, MaxHealt);
    }
}
