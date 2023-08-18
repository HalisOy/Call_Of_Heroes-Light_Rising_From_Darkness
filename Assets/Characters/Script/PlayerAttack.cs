using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Melee Attack Collider")]
    [SerializeField] private Collider2D AttackCollider;
    [SerializeField] private LayerMask EnemyLayer;
    public PowerSystem PlayerPower;
    [HideInInspector] public Animator PlayerAnimator;
    [HideInInspector] public bool SkillActive;
    [HideInInspector] public bool InputEnable;


    PlayerMovement PlayerMove;
    void Start()
    {
        PlayerPower = new PowerSystem();
        PlayerMove = GetComponent<PlayerMovement>();
        PlayerAnimator = GetComponent<Animator>();
        PlayerPower.SetPower(10);
        InputEnable = true;
    }
    private void OnAttack()
    {
        if (!SkillActive && InputEnable)
            PlayerAnimator.SetTrigger("Attack");
    }
    private void OnHeavyAttack()
    {
        if (!SkillActive && InputEnable)
            PlayerAnimator.SetTrigger("HeavyAttack");
    }

    private void OnPotion()
    {
        if (PlayerMove.Potion > 0)
        {
            PlayerMove.PotionPress();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy EnemySC) && AttackCollider.gameObject.activeSelf && collision.CompareTag("Enemy"))
        {
            PlayerMove.EnemyHit = true;
            EnemySC.Damage(PlayerPower.CurrentPower, transform);
        }
        if(collision.GetComponent<Boss>())
        {
            collision.GetComponent<Boss>().Damage(PlayerPower.CurrentPower);
        }
    }
    private void AttackAnimFinish()
    {
        PlayerMove.EnemyHit = false;
    }


}