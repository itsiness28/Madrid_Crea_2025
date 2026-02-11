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
    private PlayerMove player;
    [SerializeField]
    private Rigidbody2D rb;

    private void Update()
    {
        if (player.Velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (player.Velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        animator.SetFloat("VelocityY", rb.linearVelocityY);
        animator.SetFloat("VelocityX", Mathf.Abs(rb.linearVelocityX));
        animator.SetBool("Below", player.Below);
        
    }

    private void CheckGravity()
    {
        //if (rb.gravityScale == player.PlayerData.Gravity)
        //{
        //    spriteRenderer.color = Color.white;
        //}
        //else
        //{
        //    spriteRenderer.color = Color.red;
        //}
    }
}
