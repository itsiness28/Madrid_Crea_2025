using UnityEngine;

public class Checker : MonoBehaviour
{
    [SerializeField]
    private Collider2D _collider;
    [SerializeField]
    private LayerMask mask;

    public void Check()
    {
        Collider2D c = Physics2D.OverlapBox(_collider.bounds.center, new Vector2(_collider.bounds.size.x, _collider.bounds.size.y), 0, mask);
        if (c != null)
        {
            c.transform.position = new Vector3(_collider.bounds.center.x, c.transform.position.y) + Vector3.right;
        }
    }
    private void OnDrawGizmos()
    {

        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_collider.bounds.center, new Vector2(_collider.bounds.size.x, _collider.bounds.size.y));


    }
}
