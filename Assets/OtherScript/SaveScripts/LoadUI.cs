using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadUI : MonoBehaviour
{
    public int LoadNum;
    private void OnEnable()
    {
        SaveData Data = SaveSystem.LoadData(LoadNum);
        if (Data != null)
            transform.GetChild(0).GetComponent<TMP_Text>().text = Data.Classes.ToString();
    }
}
