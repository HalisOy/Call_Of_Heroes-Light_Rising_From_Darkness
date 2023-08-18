using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private GameObject Player;
    public HealtSystem BossHealt;
    public GameObject HealtBar;
    private Animator BossAnim;
    public Collider2D HitCollider;
    public GameObject LastStory;
    public bool IsHit;
    public bool IsPlayerDamage;
    private void Start()
    {
        BossAnim = GetComponent<Animator>();
        BossHealt.SetHealt(BossHealt.MaxHealt);
        IsHit = false;
        IsPlayerDamage = false;
    }

    private void Update()
    {
        if (IsHit)
        {
            Hit();
        }
    }

    public void Damage(int power)
    {
        if (Player == null)
        {
            Player = FindObjectOfType<PlayerMovement>().gameObject;
            HealtBar.transform.parent.gameObject.SetActive(true);
            StartCoroutine(AttackWait());
        }
        BossHealt.Damage(power);
        HealtBar.transform.GetChild(0).GetComponent<Image>().fillAmount = (float)BossHealt.Healt / (float)BossHealt.MaxHealt;
        if (BossHealt.Healt <= 0)
            BossAnim.SetTrigger("Death");

    }

    IEnumerator AttackWait()
    {
        yield return new WaitForSeconds(6f);
        if (!Player.GetComponent<PlayerMovement>().PlayerDeath)
        {
            IsPlayerDamage = false;
            BossAnim.SetTrigger("Attack");
        }
    }

    private void Hit()
    {
        if (!IsPlayerDamage)
        {
            Collider2D[] HitTarget;
            HitTarget = new Collider2D[2];
            ContactFilter2D ContactFilter = new ContactFilter2D();
            ContactFilter.useTriggers = true;
            ContactFilter.SetLayerMask(LayerMask.GetMask("Player"));
            int HitCount = Physics2D.OverlapCollider(HitCollider, ContactFilter, HitTarget);
            if (HitCount > 0)
            {
                HitTarget[0].transform.parent.GetComponent<PlayerMovement>().PlayerOnDamage(transform, 30);
                IsPlayerDamage = true;
            }
        }
    }

    public void ThisDestroy()
    {
        LastStory.SetActive(true);
        Destroy(gameObject);
    }
}
