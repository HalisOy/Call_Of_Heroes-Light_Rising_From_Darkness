using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Range(0, 10)]
    [SerializeField] private float MinMoveSpeed;
    [Range(0, 10)]
    [SerializeField] private float MaxMoveSpeed;
    [HideInInspector] public float CurrentMoveSpeed;
    [HideInInspector] public float AttackSpeed;
    [HideInInspector] public bool AttackReady;
    public bool IsAttack;
    public Collider2D Vision;
    public Collider2D AttackCollider;
    private Rigidbody2D EnemyRigidbody;
    public Animator EnemyAnimation;
    private float EnemyDirection;
    private float TargetDirection;
    public GameObject TargetAttack;
    [HideInInspector] public bool IsStun;
    [HideInInspector] public bool IsTakeHit;
    [HideInInspector] public bool IsDeath;
    void Start()
    {
        EnemyAnimation = GetComponent<Animator>();
        CurrentMoveSpeed = Random.Range(MinMoveSpeed, MaxMoveSpeed);
        AttackReady = true;
        IsStun = false;
        IsAttack = false;
        IsTakeHit = false;
        IsDeath = false;
        EnemyDirection = transform.localScale.x;
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(Patrolling), 5f, 8f);

    }

    void Update()
    {
        if (!IsDeath)
        {
            EnemyAnimation.SetFloat("Run", EnemyRigidbody.velocity.magnitude);
            if (!IsStun && !IsAttack)
                EnemyAIFunction();
            if (IsTakeHit && TargetAttack == null)
            {
                TargetAttack = GameObject.FindGameObjectWithTag("Player");
                Vision.gameObject.SetActive(false);
            }
        }
        else
            EnemyRigidbody.velocity = new Vector2(0f, 0f);
    }

    private void EnemyAIFunction()
    {
        if (TargetAttack == null)
        {
            Collider2D[] HitTarget;
            HitTarget = new Collider2D[2];
            ContactFilter2D ContactFilter = new ContactFilter2D();
            ContactFilter.useTriggers = true;
            ContactFilter.SetLayerMask(LayerMask.GetMask("Player"));
            int HitCount = Physics2D.OverlapCollider(Vision, ContactFilter, HitTarget);
            if (HitCount > 0)
            {
                TargetAttack = HitTarget[0].transform.parent.gameObject;
                Vision.gameObject.SetActive(false);
                CancelInvoke(nameof(Patrolling));
            }
        }
        else
        {
            if (!TargetAttack.GetComponent<PlayerMovement>().PlayerDeath && !IsTakeHit)
            {
                if (TargetAttack.transform.position.x - transform.position.x > 0)
                    TargetDirection = 1;
                else
                    TargetDirection = -1;

                if (TargetAttack.transform.position.x - transform.position.x > 2.7f || TargetAttack.transform.position.x - transform.position.x < -2.7f)
                {
                    if (!IsAttack)
                    {
                        EnemyRigidbody.velocity = new Vector2(CurrentMoveSpeed * TargetDirection, 0f);
                        EnemyFlip();
                    }
                    else
                        EnemyRigidbody.velocity = new Vector2(0f, 0f);
                }
                else
                {
                    if (!IsAttack && AttackReady)
                    {
                        EnemyFlip();
                        EnemyAnimation.SetTrigger("Attack");
                    }
                    EnemyRigidbody.velocity = new Vector2(0f, 0f);
                }
            }
        }
    }

    private void Patrolling()
    {
        StartCoroutine(PatrollingGo());
    }

    IEnumerator PatrollingGo()
    {
        EnemyRigidbody.velocity = new Vector2(Random.Range(-1f, 1f) * 10, 0f);
        if (EnemyRigidbody.velocity.x > 0)
            transform.localScale = new Vector2(EnemyDirection, transform.localScale.y);
        else
            transform.localScale = new Vector2(-EnemyDirection, transform.localScale.y);
        EnemyAnimation.SetFloat("Run", EnemyRigidbody.velocity.x);
        yield return new WaitForSeconds(3f);
        EnemyRigidbody.velocity = new Vector2(0, 0f);
    }

    IEnumerator AttackWait()
    {
        AttackReady = false;
        yield return new WaitForSeconds(AttackSpeed);
        AttackReady = true;
    }

    void EnemyFlip()
    {
        transform.localScale = new Vector2(EnemyDirection * TargetDirection, transform.localScale.y);
    }

    public void HitPlayer()
    {
        Collider2D[] HitTarget;
        HitTarget = new Collider2D[2];
        ContactFilter2D ContactFilter = new ContactFilter2D();
        ContactFilter.useTriggers = true;
        ContactFilter.SetLayerMask(LayerMask.GetMask("Player"));
        int HitCount = Physics2D.OverlapCollider(AttackCollider, ContactFilter, HitTarget);
        if (HitCount > 0)
        {
            if (HitTarget[0].transform.parent.GetComponentInChildren<PlayerBody>().BodyVisible)
                HitTarget[0].transform.parent.GetComponent<PlayerMovement>().PlayerOnDamage(transform,10);
        }
        AttackReady = false;
    }

    public void PlayerBoostPoint()
    {
        TargetAttack.GetComponent<PlayerMovement>().PointUp(Random.Range(50, 200));
    }
}
