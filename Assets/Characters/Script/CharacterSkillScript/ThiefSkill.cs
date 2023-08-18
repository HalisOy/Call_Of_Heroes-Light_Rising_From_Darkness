using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThiefSkill : MonoBehaviour
{
    [HideInInspector] public PlayerMovement PlayerMove;
    [HideInInspector] public PlayerBody Body;
    [HideInInspector] public PlayerAttack AttackCollider;
    [HideInInspector] public PlayerSkillCollider SkillCollider;
    [SerializeField] private CharactersSO MyCharacter;
    [SerializeField] private Image Cooltime1;
    [SerializeField] private Image Activetime1;
    [SerializeField] private Image Cooltime2;
    [SerializeField] private Image Activetime2;
    private SkillTimeSystem Active1;
    private SkillTimeSystem Active2;
    private SkillTimeSystem Active3;

    private void Start()
    {
        PlayerMove = GetComponent<PlayerMovement>();
        Body = GetComponentInChildren<PlayerBody>();
        AttackCollider = GetComponentInChildren<PlayerAttack>();
        Active1 = new SkillTimeSystem();
        Active2 = new SkillTimeSystem();
        Active3 = new SkillTimeSystem();
    }

    private void Update()
    {
        Active1Time();
        Active2Time();
        Active3Time();
    }

    void OnSkill1()
    {
        if (Active1.CoolDown <= 0)
        {
            //Skill1Performed(StartSkill);
            Active1.CoolDown = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown;
            Active1.ActiveTime = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillActiveTime;
            Active1.WorkOnce = false;
        }
    }
    void OnSkill2()
    {
        if (Active2.CoolDown <= 0)
        {
            //Skill2Performed(StartSkill);
            Active2.CoolDown = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown;
            Active2.ActiveTime = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillActiveTime;
            Active2.WorkOnce = false;
        }
    }
    void OnSkill3()
    {

    }

    private void Active1Time()
    {
        if (Active1.ActiveTime > 0)
        {
            Active1.ActiveTime -= Time.deltaTime;
            Activetime1.fillAmount = Active1.ActiveTime / MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillActiveTime;
        }
        else
        {
            if (!Active1.WorkOnce)
            {
                //Skill1Performed(FinishSkill);
                Active1.ActiveTime = 0;
                Active1.WorkOnce = true;
            }
            if (Active1.CoolDown > 0)
            {
                Active1.CoolDown -= Time.deltaTime;
                Cooltime1.fillAmount = Active1.CoolDown / MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown;
            }
        }
    }

    private void Active2Time()
    {
        if (Active2.ActiveTime > 0)
        {
            Active2.ActiveTime -= Time.deltaTime;
            Activetime2.fillAmount = Active2.ActiveTime / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillActiveTime;
        }
        else
        {
            if (!Active2.WorkOnce)
            {
                //Skill2Performed(FinishSkill);
                Active2.ActiveTime = 0;
                Active2.WorkOnce = true;
            }
            if (Active2.CoolDown > 0)
            {
                Active2.CoolDown -= Time.deltaTime;
                Cooltime2.fillAmount = Active2.CoolDown / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown;
            }
        }
    }

    private void Active3Time()
    {
        if (Active3.ActiveTime > 0)
        {
            Active3.ActiveTime -= Time.deltaTime;
            Activetime1.fillAmount = Active3.ActiveTime / MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillActiveTime;
        }
        else
        {
            if (!Active3.WorkOnce)
            {
                Skill3Performed();
                Active3.ActiveTime = 0;
                Active3.WorkOnce = true;
            }
            if (Active3.CoolDown > 0)
            {
                Active3.CoolDown -= Time.deltaTime;
                Cooltime1.fillAmount = Active3.CoolDown / MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillCoolDown;
            }
        }
    }



    void Skill1Performed()
    {

    }

    void Skill2Performed()
    {

    }
    void Skill3Performed()
    {

    }
}
