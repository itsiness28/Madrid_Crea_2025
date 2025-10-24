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
        if (player.MoveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (player.MoveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetFloat("VelocityX", Mathf.Abs(player.MoveInput));
        animator.SetBool("Below", player.IsGrounded);
        
    }

    private void CheckGravity()
    {
        if (rb.gravityScale == player.PlayerData.Gravity)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }
}
