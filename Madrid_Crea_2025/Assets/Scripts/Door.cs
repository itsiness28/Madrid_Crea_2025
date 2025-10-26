using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetPliOpenTrigger()
    {
        anim.SetTrigger("OpenTrigger");
    }
}
