using System.Collections;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetFadeOutTrigger()
    {
        anim.SetTrigger("FadeOutTrigger");
    }

    public void SetFadeOutTrigger2()
    {
        anim.SetTrigger("FadeOutTrigger");
    }

    IEnumerator time()
    {
        yield return new WaitForSeconds(2f);
        
    }
}
