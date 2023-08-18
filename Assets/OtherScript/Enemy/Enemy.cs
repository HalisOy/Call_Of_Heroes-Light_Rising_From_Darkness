using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public HealtSystem EnemyHealt;
    public GameObject HealtBar;
    private Rigidbody2D EnemyRigidbody;
    public GameObject DamageObject;
    public SpriteRenderer EnemySprite;
    EnemyAI EnemyAIScript;
    private bool IsStun;

    void Start()
    {
        EnemyAIScript = GetComponent<EnemyAI>();
        EnemyRigidbody = GetComponent<Rigidbody2D>();
        EnemyHealt.SetHealt(EnemyHealt.MaxHealt);
        IsStun = false;

    }

    public IEnumerator TimedDamage(float Time, int power)
    {
        yield return new WaitForSeconds(Time);
        if (!EnemyAIScript.IsDeath)
        {
            EnemyAIScript.EnemyAnimation.SetTrigger("TakeHit");
            DamageObject.GetComponent<Animator>().SetTrigger("WizardDamage");
            DamageNotStun(power);
        }
    }

    public void DamageNotStun(int Power)
    {
        EnemyHealt.Damage(Power);
        HealtBar.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)EnemyHealt.Healt / (float)EnemyHealt.MaxHealt;
        EnemyAIScript.EnemyAnimation.SetTrigger("TakeHit");
        if (EnemyHealt.Healt <= 0)
            Death();
    }

    public void Damage(int Power, Transform PlayerTransform)
    {
        EnemyHealt.Damage(Power);
        HealtBar.transform.GetChild(1).GetComponent<Image>().fillAmount = (float)EnemyHealt.Healt / (float)EnemyHealt.MaxHealt;
        EnemyAIScript.EnemyAnimation.SetTrigger("TakeHit");
        StartCoroutine(EnemyStun(PlayerTransform));
    }

    private IEnumerator EnemyStun(Transform PlayerTransform)
    {
        //EnemyRigidbody.velocity = new Vector2(0f, 0f);
        Vector2 StunDirection = transform.position - PlayerTransform.position;
        StunDirection.y = 0.9f;
        StunDirection.x = (StunDirection.x > 0) ? 0.7f : -0.7f;
        IsStun = true;
        EnemyAIScript.IsStun = IsStun;
        //EnemyAnimator.SetTrigger("TakeHit");
        //PlayerRigidbody.velocity = new Vector2(PlayerDirection.x * AttackForwardMoveValue, 0f);
        //EnemyRigidbody.AddForce(StunDirection * 13f, ForceMode2D.Impulse);
        EnemyRigidbody.velocity = new Vector2(StunDirection.x * 14, 0.1f);
        yield return new WaitForSeconds(0.15f);
        if (EnemyHealt.Healt < 1)
        {
            Death();
        }
        //EnemyAnimator.SetTrigger("TakeHitFinish");
        IsStun = false;
        EnemyAIScript.IsStun = IsStun;
        EnemyRigidbody.velocity = new Vector2(0f, 0f);
    }

    public void Death()
    {
        HealtBar.SetActive(false);
        EnemyAIScript.IsDeath = true;
        EnemyAIScript.PlayerBoostPoint();
        EnemyAIScript.EnemyAnimation.SetTrigger("Death");
    }

    public void ThisDestroy()
    {
        Destroy(gameObject);
    }

    public void SetDamageObject()
    {
        DamageObject = transform.GetChild(transform.childCount - 1).gameObject;
    }
}
