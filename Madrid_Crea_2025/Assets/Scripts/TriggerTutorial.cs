using UnityEngine;

public class TriggerTutorial : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sprite;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            sprite.enabled = false;
        }
    }
}
