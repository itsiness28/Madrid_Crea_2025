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

<<<<<<< HEAD
        //StartCoroutine(InstantiateShockwave());
=======
        StartCoroutine(InstantiateShockwave());
>>>>>>> 78fdaf0ad5cec79793a8e19cc145aeaf59b85b3b
    }

    public void SetGoToPresentTrigger()
    {
        transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
        anim.SetTrigger("GoToPresentTrigger");

<<<<<<< HEAD
        //StartCoroutine(InstantiateShockwave());
=======
        StartCoroutine(InstantiateShockwave());
>>>>>>> 78fdaf0ad5cec79793a8e19cc145aeaf59b85b3b
    }

    IEnumerator InstantiateShockwave()
    {
        GameObject clon = Instantiate(shockwavePrefab);
<<<<<<< HEAD
        clon.transform.position = player.transform.position;
=======
        clon.transform.position = player.transform.position + new Vector3(0, 0.5f, 0);
>>>>>>> 78fdaf0ad5cec79793a8e19cc145aeaf59b85b3b

        yield return new WaitForSeconds(2f);
        Destroy(clon);
    }
}
