using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTimeSystem
{
    public float CoolDown;
    public float ActiveTime;
    public bool WorkOnce;
    public SkillTimeSystem()
    {
        CoolDown = 0;
        ActiveTime = 0;
        WorkOnce = true;
    }
}
