using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WizardSkill : MonoBehaviour
{
    [HideInInspector] public PlayerMovement PlayerMove;
    [HideInInspector] public PlayerBody Body;
    [HideInInspector] public PlayerAttack AttackCollider;
    [SerializeField] private CharactersSO MyCharacter;
    [SerializeField] private GameObject Cooltime1;
    [SerializeField] private GameObject Cooltime2;
    [SerializeField] private Collider2D SkillCollider;
    [SerializeField] private GameObject SkillInstantiate;
    private SkillTimeSystem Active1;
    private SkillTimeSystem Active2;
    private bool SkillPress;
    public GameObject NearestEnemy;
    public GameObject TempNearestEnemy;
    public GameObject ControlEnemy;

    private void Start()
    {
        PlayerMove = GetComponent<PlayerMovement>();
        Body = GetComponentInChildren<PlayerBody>();
        AttackCollider = GetComponentInChildren<PlayerAttack>();
        Active1 = new SkillTimeSystem();
        Active2 = new SkillTimeSystem();
        NearestEnemy = null;
        TempNearestEnemy = null;
    }

    private void Update()
    {
        Active1Time();
        Active2Time();
        SkillSelectedCharacter();
    }

    void OnSkill1()
    {
        if (Active1.CoolDown <= 0)
        {
            AttackCollider.PlayerAnimator.SetTrigger("OnSkill1");
            Cooltime1.transform.GetChild(2).gameObject.SetActive(true);
            Active1.CoolDown = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown;
        }
    }
    void OnSkill2(InputValue Value)
    {
        if (Active2.CoolDown <= 0)
            if (Value.isPressed)
            {
                AttackCollider.SkillActive = true;
                SkillCollider.gameObject.SetActive(true);
                SkillPress = true;
            }
            else
            {
                AttackCollider.SkillActive = false;
                SkillCollider.gameObject.SetActive(false);
                SkillPress = false;
                Skill2Performed();
                Cooltime2.transform.GetChild(2).gameObject.SetActive(true);
                Active2.CoolDown = MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown;
            }
    }
    void OnSkill3(InputValue Value)
    {
        /*if (Active3.CoolDown <= 0)
            if (Value.isPressed)
            {
                AttackCollider.SkillActive = true;
                SkillCollider.gameObject.SetActive(true);
                SkillPress = true;
            }
            else
            {
                AttackCollider.SkillActive = false;
                SkillCollider.gameObject.SetActive(false);
                SkillPress = false;
                Skill3Performed(false);
                Active3.CoolDown = MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillCoolDown;
                Active3.ActiveTime = MyCharacter.SkillTree.Active3.SkillLevels[MyCharacter.SkillTree.Active3.SkillLevel].SkillActiveTime;
                Active3.WorkOnce = false;
            }*/
    }

    private void Active1Time()
    {
        if (Active1.CoolDown > 0)
        {
            Active1.CoolDown -= Time.deltaTime;
            Cooltime1.transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - (Active1.CoolDown / MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillCoolDown);
            Cooltime1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active1.CoolDown.ToString("F0");
        }
        else
            Cooltime1.transform.GetChild(2).gameObject.SetActive(false);
    }

    private void Active2Time()
    {
        if (Active2.CoolDown > 0)
        {
            Active2.CoolDown -= Time.deltaTime;
            Cooltime2.transform.GetChild(1).GetComponent<Image>().fillAmount = 1 - (Active2.CoolDown / MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillCoolDown);
            Cooltime2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Active2.CoolDown.ToString("F0");
        }
        else
            Cooltime2.transform.GetChild(2).gameObject.SetActive(false);
    }

    public void Skill1Performed()
    {
        GameObject Skill1 = Instantiate(MyCharacter.SkillTree.Active1.SkillPrefabs, SkillInstantiate.gameObject.transform.position, Quaternion.identity);
        Skill1.GetComponent<SkillShotSC>().Power = MyCharacter.SkillTree.Active1.SkillLevels[MyCharacter.SkillTree.Active1.SkillLevel].SkillPower * AttackCollider.PlayerPower.CurrentPower;
        Skill1.GetComponent<SkillShotSC>().SkillSpeed = Skill1.GetComponent<SkillShotSC>().SkillSpeed * PlayerMove.PlayerDirection.x;
        Skill1.GetComponent<SkillShotSC>().SkillDirection = PlayerMove.PlayerDirection;
    }

    void Skill2Performed()
    {
        if (NearestEnemy != null)
        {
            StartCoroutine(NearestEnemy.GetComponent<Enemy>().TimedDamage(0.1f, AttackCollider.PlayerPower.CurrentPower * MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillPower));
            StartCoroutine(NearestEnemy.GetComponent<Enemy>().TimedDamage(1f, AttackCollider.PlayerPower.CurrentPower * (MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillPower + 1)));
            StartCoroutine(NearestEnemy.GetComponent<Enemy>().TimedDamage(2f, AttackCollider.PlayerPower.CurrentPower * (MyCharacter.SkillTree.Active2.SkillLevels[MyCharacter.SkillTree.Active2.SkillLevel].SkillPower + 2)));
            Instantiate(MyCharacter.SkillTree.Active2.SkillPrefabs, NearestEnemy.transform);
            NearestEnemy.GetComponent<Enemy>().SetDamageObject();
            NearestEnemy.GetComponent<Enemy>().EnemySprite.color = Color.white;
            NearestEnemy = null;
        }
    }
    void Skill3Performed(bool FinishTime)
    {
        /*if (!FinishTime)
        {
            NearestEnemy.GetComponent<EnemyAI>().Vision.gameObject.SetActive(true);
            NearestEnemy.GetComponent<EnemyAI>().TargetAttack = null;
            NearestEnemy.GetComponent<Enemy>().EnemySprite.color = Color.white;
            ControlEnemy = NearestEnemy;
            NearestEnemy = null;
            ControlEnemy.layer = LayerMask.NameToLayer("Player");
            ControlEnemy.tag = "Player";

            //attack enemy
        }
        else if (FinishTime)
        {
            ControlEnemy.GetComponent<Enemy>().ImmediateDeath();
            ControlEnemy = null;
        }*/
    }

    void SkillSelectedCharacter()
    {
        if (SkillPress)
        {
            float NearestDistance = float.MaxValue;
            Collider2D[] HitColliders;
            HitColliders = new Collider2D[20];
            ContactFilter2D ContactFilter = new ContactFilter2D();
            ContactFilter.useTriggers = true;
            ContactFilter.SetLayerMask(LayerMask.GetMask("Enemy"));
            int HitCount = Physics2D.OverlapCollider(SkillCollider, ContactFilter, HitColliders);
            if (HitCount > 0)
            {
                for (int i = 0; i < HitCount; i++)
                {
                    if (Vector2.Distance(transform.position, HitColliders[i].transform.position) < NearestDistance)
                    {
                        NearestDistance = Vector2.Distance(transform.position, HitColliders[i].transform.position);
                        NearestEnemy = HitColliders[i].gameObject;
                        if (NearestEnemy != TempNearestEnemy)
                        {
                            if (TempNearestEnemy != null)
                                TempNearestEnemy.GetComponent<Enemy>().EnemySprite.color = Color.white;
                            TempNearestEnemy = NearestEnemy;

                        }
                    }
                }
                if (NearestEnemy != null)
                {
                    NearestEnemy.GetComponent<Enemy>().EnemySprite.color = Color.red;
                    for (int i = 0; i < HitCount; i++)
                    {
                        if (HitColliders[i].gameObject != NearestEnemy)
                            HitColliders[i].GetComponent<Enemy>().EnemySprite.color = Color.white;
                    }
                }
            }
            else
            {
                NearestEnemy = null;
                if (TempNearestEnemy != null)
                {
                    TempNearestEnemy.GetComponent<Enemy>().EnemySprite.color = Color.white;
                    TempNearestEnemy = null;
                }
            }
        }

    }
}
