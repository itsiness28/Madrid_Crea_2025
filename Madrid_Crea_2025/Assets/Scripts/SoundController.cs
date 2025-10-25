using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource sonido;


    public void PlaySonido(AudioClip clip)
    {
        sonido.PlayOneShot(clip);
    }
}
