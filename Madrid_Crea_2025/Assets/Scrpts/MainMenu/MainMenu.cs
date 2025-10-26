using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private VolumenConfigurationSO v;
    public void PlayGame(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }

    public void MoreV()
    {
        if (v.Volume + 0.1f <= 1)
        {
            v.Volume += 0.1f;
        }
    }

    public void LessV()
    {
        if (v.Volume - 0.1f >= 0)
        {
            v.Volume -= 0.1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

    
