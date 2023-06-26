using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void DEBUGPlay()
    {
        SceneManager.LoadScene("DEBUGLEVEL");
    }

    public void Credits()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
#if UNITY_PSP2
        UnityEngine.PSVita.Utils.PSP2Utility.QuitGame();
#else
        Application.Quit();
#endif
    }
}
