using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void PlayGame()
    {
        SceneManager.LoadScene("1_Ilustracion");
    }

    public void Options()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

    
