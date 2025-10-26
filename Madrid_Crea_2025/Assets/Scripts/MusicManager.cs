using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource sonido;
    [SerializeField] AudioClip musica1;

    public static MusicManager Instance;

    [SerializeField]
    private VolumenConfigurationSO v;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        sonido.volume = /*0.1f **/ v.Volume;
    }
}
