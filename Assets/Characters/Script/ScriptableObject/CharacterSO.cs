using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInfo", menuName = "ScriptableObject/Character")]
public class CharactersSO : ScriptableObject
{
    public string Name;
    public GameObject CharacterPrefab;
    public CharacterClass Classes;
    public SkillTreeSO SkillTree;

    public enum CharacterClass
    {
        Warrior,
        Thief,
        Wizard,
        Priest
    }
}