using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        SceneManager.LoadScene("Arena");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Sigma()
    {
        Application.Quit();
    }
}
