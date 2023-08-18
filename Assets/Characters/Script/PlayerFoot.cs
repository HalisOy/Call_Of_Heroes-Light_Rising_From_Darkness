using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    private PlayerMovement PlayerMove;
    private Animator PlayerAnimator;

    private void Start()
    {
        PlayerMove = GetComponentInParent<PlayerMovement>();
        PlayerAnimator = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            PlayerMove.IsGround = true;
            PlayerAnimator.SetBool("Jump", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            PlayerMove.IsGround = false;
        }
    }
}
