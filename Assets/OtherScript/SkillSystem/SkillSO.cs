using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObject/SkillSystem/Skill")]
public class SkillSO : ScriptableObject
{
    public Sprite SkillImage;
    public string Name;
    [field: TextArea]
    public string Description;
    public int SkillLevel;
    public GameObject SkillPrefabs;
    public List<SkillLevelData> SkillLevels;
}
