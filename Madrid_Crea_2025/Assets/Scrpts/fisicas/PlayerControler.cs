using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PlayerControler : RaycastController
{
    [SerializeField]
    private float maxSlopeAngle = 55f;

    [SerializeField]
    private int horoizontalCornerCorrection;

    [SerializeField]
    private int verticalCornerCorrection;

    [SerializeField]
    private bool see;

    [SerializeField]
    private CollisionInfo collisions;

    public override void Start()
    {

        base.Start();


    }

    public void Move(Vector2 moveAmount)
    {

        UpdateRaycastOrigins();
        collisions.ResetHorizontal();
        collisions.MoveAmountOld = moveAmount;

        if (moveAmount.x != 0)
        {

            HorizontalColision(ref moveAmount);

        }
        collisions.ResetVertical();
        if (moveAmount.y != 0)
        {

            VerticalCollision(ref moveAmount);

        }
        transform.Translate(moveAmount);

    }

    void HorizontalColision(ref Vector2 moveAmount)
    {

        float directionX = Mathf.Sign(moveAmount.x);
        float rayLengh = Mathf.Abs(moveAmount.x) + skinWidh;
        bool cornerCorrection = true;
        int subsRays = ((Mathf.Sign(moveAmount.y) == -1 && !getInfoCollision().Below) ? horoizontalCornerCorrection: 0 );
        for (int i = 0; i < horizontalRayCount - subsRays; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft + new Vector2(0, horizontalRaySpacing * subsRays) : raycastOrigins.bottomRight + new Vector2(0, horizontalRaySpacing * subsRays);
            rayOrigin += Vector2.up * horizontalRaySpacing * i;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLengh, collisionMask);

            if (hit)
            {
                moveAmount.x = (hit.distance - skinWidh) * directionX;
                rayLengh = hit.distance;


                collisions.Left = directionX == -1;
                collisions.Right = directionX == 1;
                cornerCorrection = false;


            }


            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLengh, Color.red);

        }

        if (cornerCorrection == true && Mathf.Sign(moveAmount.y) == -1 && !getInfoCollision().Below)
        {
            CastHorizontalCornerCorrection(directionX, rayLengh, ref moveAmount, horoizontalCornerCorrection - 1, (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
        }
    }

    void VerticalCollision(ref Vector2 moveAmount)
    {

        float directionY = Mathf.Sign(moveAmount.y);
        float rayLengh = Mathf.Abs(moveAmount.y) + skinWidh;

        bool cornerCorrection = (directionY == 1);

        int rayCount = (directionY == -1) ? verticalRayCount : verticalRayCount - (verticalCornerCorrection * 2);

        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft + new Vector2(verticalRaySpacing * verticalCornerCorrection, 0);
            rayOrigin += Vector2.right * verticalRaySpacing * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLengh, collisionMask);

            if (hit)
            {

                moveAmount.y = (hit.distance - skinWidh) * directionY;
                rayLengh = hit.distance;

                collisions.Below = directionY == -1;
                collisions.Above = directionY == 1;
                cornerCorrection = false;
            }
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLengh, Color.red);

        }
        if (cornerCorrection == true)
        {
            CastVerticalCornerCorrection(rayLengh, ref moveAmount, verticalCornerCorrection - 1, raycastOrigins.topLeft);
            CastVerticalCornerCorrection(rayLengh, ref moveAmount, verticalCornerCorrection - 1, raycastOrigins.topRight);
        }

    }

    private void CastHorizontalCornerCorrection(float direction, float rayLengh, ref Vector2 moveAmount, int rayNumbers, Vector2 rayOrigin)
    {
        
        
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin + Vector2.up * horizontalRaySpacing * rayNumbers, Vector2.right * direction, rayLengh, collisionMask);
        if (hit)
        {
            moveAmount.y = (rayNumbers - 1) * horizontalRaySpacing + skinWidh;
        }
        else if (rayNumbers > 0)
        {
            CastHorizontalCornerCorrection(direction, rayLengh, ref moveAmount, rayNumbers - 1, rayOrigin);
        }
        if (see)
            Debug.DrawRay(rayOrigin + Vector2.up * horizontalRaySpacing * rayNumbers , Vector2.right  * direction * rayLengh, Color.blue);
    }

    private void CastVerticalCornerCorrection(float rayLengh, ref Vector2 moveAmount, int rayNumbers, Vector2 rayOrigin)
    {


        RaycastHit2D hit = Physics2D.Raycast(rayOrigin +  Vector2.right * horizontalRaySpacing * rayNumbers * (rayOrigin == raycastOrigins.topLeft ? 1 : -1), Vector2.up, rayLengh, collisionMask);
        if (hit)
        {

            int mod = (rayOrigin == raycastOrigins.topLeft) ? 1 : -1;
            moveAmount.x += (rayNumbers * verticalRaySpacing + skinWidh) * mod;


        }
        else if (rayNumbers > 0)
        {
            CastVerticalCornerCorrection(rayLengh, ref moveAmount, rayNumbers - 1, rayOrigin);
        }
        if (see)
            Debug.DrawRay(rayOrigin + Vector2.right * horizontalRaySpacing * rayNumbers * (rayOrigin == raycastOrigins.topLeft ? 1 : -1),  Vector2.up * rayLengh, (raycastOrigins.topLeft == rayOrigin) ? Color.yellow : Color.blue);
    }
    public CollisionInfo getInfoCollision()
    {

        return collisions;

    }

    [Serializable]
    public struct CollisionInfo
    {

        private bool below;
        private bool right;
        private bool above;
        private bool left;

        private Vector3 moveAmountOld;

        public bool Above { get => above; set => above = value; }
        public bool Below { get => below; set => below = value; }
        public bool Left { get => left; set => left = value; }
        public bool Right { get => right; set => right = value; }

        public Vector3 MoveAmountOld { get => moveAmountOld; set => moveAmountOld = value; }

        public void ResetHorizontal()
        {


            left = right = false;

        }

        public void ResetVertical()
        {
            above = below = false;
        }

    }

}
