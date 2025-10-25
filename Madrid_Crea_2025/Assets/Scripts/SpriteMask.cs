using System.Collections;
using UnityEngine;

public class SpriteMask : MonoBehaviour
{

    [SerializeField] Player_Movement player;
    [SerializeField] GameObject shockwavePrefab;
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
        transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
        anim.SetTrigger("GoToPastTrigger");

        StartCoroutine(InstantiateShockwave());
    }

    public void SetGoToPresentTrigger()
    {
        transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
        anim.SetTrigger("GoToPresentTrigger");

        StartCoroutine(InstantiateShockwave());
    }

    IEnumerator InstantiateShockwave()
    {
        GameObject clon = Instantiate(shockwavePrefab);
        clon.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);

        yield return new WaitForSeconds(2f);
        Destroy(clon);
    }
}
