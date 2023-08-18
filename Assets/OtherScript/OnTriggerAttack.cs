using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerAttack : MonoBehaviour
{
    [HideInInspector] public int AttackPower;
    [HideInInspector] public int PlayerPower;
    [HideInInspector] public int HitPower;
    [HideInInspector] public Calc Operation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameObject.activeSelf && collision.GetComponent<Enemy>())
        {
            Debug.Log(collision.GetComponent<Enemy>().name);
            switch (Operation)
            {
                case Calc.Plus:
                    HitPower = AttackPower + PlayerPower;
                    collision.GetComponent<Enemy>().DamageNotStun(HitPower);
                    break;
                case Calc.Minus:
                    HitPower = AttackPower - PlayerPower;
                    collision.GetComponent<Enemy>().DamageNotStun(HitPower);
                    break;
                case Calc.Multiply:
                    HitPower = AttackPower * PlayerPower;
                    collision.GetComponent<Enemy>().DamageNotStun(HitPower);
                    break;
                case Calc.Divide:
                    HitPower = AttackPower / PlayerPower;
                    collision.GetComponent<Enemy>().DamageNotStun(HitPower);
                    break;
            }
        }
    }

    public enum Calc
    {
        Plus,
        Minus,
        Multiply,
        Divide
    }
}
