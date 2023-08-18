using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill Tree", menuName = "ScriptableObject/SkillSystem/SkillTree")]
public class SkillTreeSO : ScriptableObject
{
    public SkillSO Passive;
    public SkillSO Active1;
    public SkillSO Active2;
    public SkillSO Active3;
}
