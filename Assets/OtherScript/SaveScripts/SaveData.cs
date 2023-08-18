using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public CharactersSO.CharacterClass Classes;
    public int Point;
    public int Healt;
    public int MaxHealt;
    public int LastSceneIndex;
    public int LastCheckpoint;
    public int PassiveLevel;
    public int Active1Level;
    public int Active2Level;
    public int Active3Level;

    public SaveData(CharactersSO CharacterData,PlayerMovement Player)
    {
        Classes = CharacterData.Classes;
        Point = Player.PlayerPoint.Point;
        Healt = Player.PlayerHealt.Healt;
        MaxHealt = Player.PlayerHealt.MaxHealt;
        PassiveLevel = CharacterData.SkillTree.Passive.SkillLevel;
        Active1Level = CharacterData.SkillTree.Active1.SkillLevel;
        Active2Level = CharacterData.SkillTree.Active2.SkillLevel;
        Active3Level = CharacterData.SkillTree.Active3.SkillLevel;
    }
}