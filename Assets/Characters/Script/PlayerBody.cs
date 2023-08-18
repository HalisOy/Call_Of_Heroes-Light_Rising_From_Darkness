using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    private PlayerMovement PlayerMove;
    [HideInInspector] public bool BodyVisible;

    private void Start()
    {
        PlayerMove = GetComponentInParent<PlayerMovement>();
        BodyVisible = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boss"))
            PlayerMove.PlayerOnDamage(collision.transform, 15);
        if (collision.CompareTag("FinishMap"))
        {
            GameManager.NextScene();
        }
    }
}
