using UnityEngine;

public class SpriteMask : MonoBehaviour
{

    [SerializeField] Player_Movement player;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGoToPastAnimTrigger()
    {
        transform.position = player.transform.position;
        anim.SetTrigger("GoToPastTrigger");
    }

    public void SetGoToPresentTrigger()
    {
        transform.position = player.transform.position;
        anim.SetTrigger("GoToPresentTrigger");
    }
}
