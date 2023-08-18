using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    // Gravity Scale = 2
    private Rigidbody2D PlayerRigidbody;
    PlayerAttack PlayerAttackSC;

    public float GetMoveSpeed { get { return MoveSpeed; } }
    [SerializeField] private float MoveSpeed = 10;
    [SerializeField] private float JumpSpeed = 22;
    [SerializeField] private float DashPower = 3;
    [SerializeField] private float DashingTime = 0.15f;
    [SerializeField] private float DashingCooldown = 0.5f;
    public HealtSystem PlayerHealt;
    public GameObject HealtBar;
    public PointSystem PlayerPoint;
    public GameObject PointBar;
    public int Potion;
    public GameObject PotionUI;
    [HideInInspector] public float CurrentMoveSpeed;
    public Canvas SkillTree;
    public GameObject DeathUI;

    private Animator PlayerAnimator;
    [HideInInspector] public Vector2 PlayerDirection;
    [HideInInspector] public float Playerx;
    private Vector2 MoveInput;
    // Dash
    private bool CanDash;
    [HideInInspector] public bool IsDashing;
    [HideInInspector] public bool IsAttacking;
    [HideInInspector] public bool InputEnable;
    public bool IsGround { get; set; }
    private bool IsStun;
    [HideInInspector] private float AttackForwardMoveValue = 14f;
    [HideInInspector] public bool EnemyHit;
    public bool PlayerDeath;
    private float PotionCoolDown;


    void Start()
    {
        PlayerAttackSC = GetComponent<PlayerAttack>();
        PotionCoolDown = 30f;
        Potion = 2;
        PotionUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Potion.ToString();
        PlayerPoint = new PointSystem();
        PlayerPoint.SetPoint(1500);
        PointBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerPoint.Point.ToString();
        Playerx = transform.localScale.x;
        CurrentMoveSpeed = MoveSpeed;
        PlayerRigidbody = GetComponent<Rigidbody2D>();
        PlayerAnimator = GetComponentInChildren<Animator>();
        PlayerDirection = new Vector2(1f, 0f);
        IsGround = true;
        CanDash = true;
        IsDashing = false;
        PlayerDeath = false;
        InputEnable = true;
    }
    void Update()
    {
        Run();
        AttackForwardMove();
        PositionTime();
    }


    private void OnMove(InputValue value)
    {
        if (InputEnable)
            MoveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        if (!IsDashing && !IsStun && !IsAttacking && InputEnable)
        {
            Vector2 PlayerVelocity = new Vector2(MoveInput.x * CurrentMoveSpeed, PlayerRigidbody.velocity.y);
            PlayerRigidbody.velocity = PlayerVelocity;
            PlayerAnimator.SetFloat("Run", PlayerRigidbody.velocity.magnitude);
            Flip();
            PlayerAnimator.SetFloat("Fall", PlayerRigidbody.velocity.y);
        }
    }

    private void OnJump()
    {
        if (IsGround && InputEnable)
        {
            PlayerRigidbody.velocity += new Vector2(0f, JumpSpeed);
            PlayerAnimator.SetTrigger("Jump");
        }
    }


    private void OnDash()
    {
        if (CanDash && InputEnable)
            StartCoroutine(Dash());
    }

    private void OnSkillTree()
    {
        if (SkillTree.gameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            InputEnable = true;
            PlayerAttackSC.InputEnable = true;
            SkillTree.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
        else if (!SkillTree.gameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            InputEnable = false;
            PlayerAttackSC.InputEnable = false;
            SkillTree.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnEscape()
    {
        Cursor.lockState = CursorLockMode.None;
        GameManager.HomeScene();
    }

    private void Flip()
    {
        if (MoveInput.x > 0)
        {
            transform.localScale = new Vector2(Playerx, transform.localScale.y);
            PlayerDirection = new Vector2(1, 0);
        }
        if (MoveInput.x < 0)
        {
            transform.localScale = new Vector2(-Playerx, transform.localScale.y);
            PlayerDirection = new Vector2(-1, 0);
        }
    }

    private IEnumerator Dash()
    {
        CanDash = false;
        IsDashing = true;
        float OriginalGravity = PlayerRigidbody.gravityScale;
        PlayerRigidbody.gravityScale = 0f;
        PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x * DashPower, 0f);
        yield return new WaitForSeconds(DashingTime);
        PlayerRigidbody.gravityScale = OriginalGravity;
        IsDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        CanDash = true;
    }

    private IEnumerator Stun(Transform EnemyTransform)
    {
        PlayerRigidbody.velocity = new Vector2(0f, 0f);
        Vector2 StunDirection = transform.position - EnemyTransform.position;
        StunDirection.y = 0.9f;
        StunDirection.x = (StunDirection.x > 0) ? 0.7f : -0.7f;
        IsStun = true;
        PlayerAnimator.SetTrigger("TakeHit");
        PlayerRigidbody.AddForce(StunDirection * 13f, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.4f);
        if (PlayerHealt.Healt < 1)
        {
            PlayerDeath = true;
            InputsEnabled(false);
            PlayerAnimator.SetTrigger("Death");
        }
        PlayerAnimator.SetTrigger("TakeHitFinish");
        IsStun = false;
    }

    public void PlayerOnDamage(Transform EnemyTransform,int Power)
    {

        PlayerHealt.Damage(Power);
        HealtBar.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)PlayerHealt.Healt / (float)PlayerHealt.MaxHealt;
        HealtBar.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)PlayerHealt.Healt / (float)PlayerHealt.MaxHealt;
        StartCoroutine(Stun(EnemyTransform));
    }

    public void InputsEnabled(bool EnabledData)
    {
        GetComponent<PlayerInput>().enabled = EnabledData;
    }

    public void AttackForwardMove()
    {
        if (IsAttacking && !IsStun && EnemyHit)
            PlayerRigidbody.velocity = new Vector2(PlayerDirection.x * AttackForwardMoveValue, 0f);
    }

    public void PotionPress()
    {
        if (PlayerHealt.Healt != PlayerHealt.MaxHealt)
        {
            Potion--;
            PotionUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Potion.ToString();
            PlayerHealt.HealtBoost(30);
            HealtBar.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)PlayerHealt.Healt / (float)PlayerHealt.MaxHealt;
            HealtBar.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)PlayerHealt.Healt / (float)PlayerHealt.MaxHealt;
        }
    }

    public void PointUp(int GetPoint)
    {
        PlayerPoint.PointPlus(GetPoint);
        PointBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerPoint.Point.ToString();
    }

    private void PositionTime()
    {
        if (Potion < 2)
        {
            if (PotionCoolDown >= 0)
            {
                PotionCoolDown -= Time.deltaTime;
                PotionUI.transform.GetChild(2).GetComponent<Image>().fillAmount = 1 - (PotionCoolDown / 30);
            }
            else
            {
                Potion++;
                PointBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PlayerPoint.Point.ToString();
                PotionCoolDown = 30;
            }
        }
    }

    public void ActiveNextSkill(GameObject NextSkill)
    {
        NextSkill.GetComponent<Button>().interactable = true;
    }

    public void SetDeathUIActive()
    {
        Cursor.lockState = CursorLockMode.None;
        DeathUI.SetActive(true);
    }
    public void SetDeathUIDeactive()
    {
        DeathUI.SetActive(false);
    }
}