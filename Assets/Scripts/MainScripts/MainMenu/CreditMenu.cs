using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditMenu : MonoBehaviour
{
    public void Exit()
    {
       SceneManager.LoadScene("MainMenu");
    }
}
