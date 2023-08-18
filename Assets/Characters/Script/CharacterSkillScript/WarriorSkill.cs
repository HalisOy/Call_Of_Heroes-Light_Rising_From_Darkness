using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WarriorSkill : MonoBehaviour
{
    [HideInInspector] public PlayerMovement PlayerMove;
    [HideInInspector] public PlayerBody Body;
    [HideInInspector] public PlayerAttack AttackCollider;
    [HideInInspector] public PlayerSkillCollider SkillCollider;
    [SerializeField] private CharactersSO MyCharacter;
    [SerializeField] private GameObject Skill3;
    [SerializeField] private GameObject Cooltime1;
    [SerializeField] private GameObject Cooltime2;
    [SerializeField] private GameObject Cooltime3;
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
            Skill1Performed(PlayerMove.GetMoveSpeed + ((PlayerMove.GetMoveSpeed * MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillPower) * 0.01f),
                AttackCollider.PlayerAnimator.GetFloat("AttackSpeed") + ((AttackCollider.PlayerAnimator.GetFloat("AttackSpeed") * MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillPower) * 0.01f));
            Cooltime1.transform.GetChild(2).gameObject.SetActive(true);
            Active1.CoolDown = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown;
            Active1.ActiveTime = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillActiveTime;
            Active1.WorkOnce = false;
        }
    }
    void OnSkill2()
    {
        if (Active2.CoolDown <= 0)
        {
            Skill2Performed(false);
            Cooltime2.transform.GetChild(2).gameObject.SetActive(true);
            Active2.CoolDown = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown;
            Active2.ActiveTime = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillActiveTime;
            Active2.WorkOnce = false;
        }
    }
    void OnSkill3(InputValue value)
    {
        if (Active3.CoolDown <= 0)
        {
            if (value.isPressed)
            {
                Skill3Performed();
                Cooltime3.transform.GetChild(2).gameObject.SetActive(true);
                Active3.CoolDown = MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillCoolDown;
            }
        }
    }

    private void Active1Time()
    {
        if (Active1.ActiveTime > 0)
        {
            Active1.ActiveTime -= Time.deltaTime;
            Cooltime1.transform.GetChild(1).GetComponent<Image>().fillAmount = Active1.ActiveTime / MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillActiveTime;
            Cooltime1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active1.ActiveTime.ToString("F0");
        }
        else
        {
            if (!Active1.WorkOnce)
            {
                Skill1Performed(PlayerMove.GetMoveSpeed, 1f);
                Active1.ActiveTime = 0;
                Active1.WorkOnce = true;
            }
            if (Active1.CoolDown > 0)
            {
                Active1.CoolDown -= Time.deltaTime;
                Cooltime1.transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - (Active1.CoolDown / MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown);
                Cooltime1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active1.CoolDown.ToString("F0");
            }
            else
            {
                Cooltime1.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    private void Active2Time()
    {
        if (Active2.ActiveTime > 0)
        {
            Active2.ActiveTime -= Time.deltaTime;
            Cooltime2.transform.GetChild(1).GetComponent<Image>().fillAmount = Active2.ActiveTime / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillActiveTime;
            Cooltime2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active2.ActiveTime.ToString("F0");
        }
        else
        {
            if (!Active2.WorkOnce)
            {
                Skill2Performed(true);
                Active2.ActiveTime = 0;
                Active2.WorkOnce = true;
            }
            if (Active2.CoolDown > 0)
            {
                Active2.CoolDown -= Time.deltaTime;
                Cooltime2.transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - (Active2.CoolDown / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown);
                Cooltime2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active2.CoolDown.ToString("F0");
            }
            else
            {
                Cooltime2.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    private void Active3Time()
    {
        if (Active3.ActiveTime > 0)
        {
            Active3.ActiveTime -= Time.deltaTime;
            Cooltime3.transform.GetChild(1).GetComponent<Image>().fillAmount = Active3.ActiveTime / MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillActiveTime;
            Cooltime3.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active3.ActiveTime.ToString("F0");
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
                Cooltime3.transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - (Active3.CoolDown / MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillCoolDown);
                Cooltime3.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active3.CoolDown.ToString("F0");
            }
            else
            {
                Cooltime3.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }



    void Skill1Performed(float ChangeSpeed, float ChangeAttackSpeed)
    {
        PlayerMove.CurrentMoveSpeed = ChangeSpeed;
        AttackCollider.PlayerAnimator.SetFloat("AttackSpeed", ChangeAttackSpeed);
    }

    void Skill2Performed(bool Visible)
    {
        Body.BodyVisible = Visible;
    }
    void Skill3Performed()
    {
        AttackCollider.PlayerAnimator.SetTrigger("Skill3");
        Skill3.GetComponent<OnTriggerAttack>().AttackPower = MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillPower;
        Skill3.GetComponent<OnTriggerAttack>().PlayerPower = AttackCollider.PlayerPower.CurrentPower;
        Skill3.GetComponent<OnTriggerAttack>().Operation = OnTriggerAttack.Calc.Multiply;
    }
}