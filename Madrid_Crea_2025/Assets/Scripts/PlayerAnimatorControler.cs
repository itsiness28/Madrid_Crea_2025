using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimatorControler : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Player_Movement player;
    [SerializeField]
    private Rigidbody2D rb;

    private void Update()
    {
        if (rb.linearVelocityX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (rb.linearVelocityX > 0)
        {
            spriteRenderer.flipX = false;
        }
        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetFloat("VelocityX", Mathf.Abs(player.MoveInput));
        animator.SetBool("Below", player.IsGrounded);
    }
}
