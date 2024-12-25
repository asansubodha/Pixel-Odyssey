using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class SceneManagement : MonoBehaviour
{
    public void LoadLevel2()
    {
        SceneManager.LoadScene("level2");
    }
    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("level1");
    }
    public void doExitGame()
    {
        Application.Quit();
    }
}

