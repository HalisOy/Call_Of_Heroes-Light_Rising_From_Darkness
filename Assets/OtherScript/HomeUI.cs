using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine.SceneManagement;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    private CharactersSO.CharacterClass CharacterClasses;
    public CharactersSO Character;
    public void NewSave(int SaveNum)
    {
        if (SaveSystem.LoadData(SaveNum) == null)
        {
            Character = Resources.Load<CharactersSO>($"{CharacterClasses}/CharacterInfo");
            PlayerMovement PlayerData = Character.CharacterPrefab.GetComponent<PlayerMovement>();
            PlayerData.PlayerHealt = new HealtSystem();
            PlayerData.PlayerPoint = new PointSystem();
            PlayerData.PlayerHealt.MaxHealt = 100;
            PlayerData.PlayerHealt.SetHealt(100);
            PlayerData.PlayerPoint.Point = 0;
            Character.SkillTree.Passive.SkillLevel = 0;
            Character.SkillTree.Active1.SkillLevel = 0;
            Character.SkillTree.Active2.SkillLevel = 0;
            Character.SkillTree.Active3.SkillLevel = 0;
            SaveSystem.NewSaveData(SaveNum, Character, PlayerData);
            CurrentCharacter.Character = Character;
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("Bu Save dosyasý dolu!");
        }

    }
    public void LoadCharacter(int LoadNum)
    {
        SaveData Data = SaveSystem.LoadData(LoadNum);
        Character = Resources.Load<CharactersSO>($"{Data.Classes}/CharacterInfo");
        CurrentCharacter.Character = Character;
        PlayerMovement PlayerData = Character.CharacterPrefab.GetComponent<PlayerMovement>();
        PlayerData.PlayerHealt = new HealtSystem();
        PlayerData.PlayerPoint = new PointSystem();
        PlayerData.PlayerHealt.MaxHealt = Data.MaxHealt;
        PlayerData.PlayerHealt.SetHealt(Data.Healt);
        PlayerData.PlayerPoint.Point = Data.Point;
        //Character.CurrentPassive = Character.SkillTree.Passive[Data.PassiveLevel];
        SceneManager.LoadScene(1);
    }

    private void CharacterChangeWarrior()
    {
        CharacterClasses = CharactersSO.CharacterClass.Warrior;
    }
    private void CharacterChangeWizard()
    {
        CharacterClasses = CharactersSO.CharacterClass.Wizard;
    }
    private void CharacterChangeThief()
    {
        CharacterClasses = CharactersSO.CharacterClass.Thief;
    }
    private void CharacterChangePriest()
    {
        CharacterClasses = CharactersSO.CharacterClass.Priest;
    }

    public void GameClose()
    {
        Application.Quit();
    }
}
