using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]

public class RaycastController : MonoBehaviour
{
    [SerializeField]
    protected LayerMask collisionMask;

    [SerializeField]
    protected float skinWidh = 0.015f;

    private const float dstBetweenRaySpacing = 0.05f;
    
    protected int horizontalRayCount;//toketeo

    protected int verticalRayCount;


    protected float horizontalRaySpacing;
    
    protected float verticalRaySpacing;

    [SerializeField]
    protected BoxCollider2D _collider;

    public RaycastOrigins raycastOrigins;

    public BoxCollider2D Collider { get => _collider; set => _collider = value; }

    public virtual void Start()
    {
        
        CalculateRaySpacing();
        
    }

    public void UpdateRaycastOrigins()
    {

        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidh * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {

        Bounds bounds = _collider.bounds;
        bounds.Expand(skinWidh * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;


        horizontalRayCount = Mathf.RoundToInt(boundsHeight / dstBetweenRaySpacing);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / dstBetweenRaySpacing);

        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);

    }



    public struct RaycastOrigins
    {

        public Vector2 topLeft, topRight;

        public Vector2 bottomLeft, bottomRight;

    }

    public int getInfo()
    {

        return horizontalRayCount;

    }
}
