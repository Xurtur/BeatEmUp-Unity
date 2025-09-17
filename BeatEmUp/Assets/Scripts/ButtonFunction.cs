using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunction : MonoBehaviour
{
  
    [SerializeField] string scene;
    public void start()
    {
        SceneManager.LoadScene(scene);
    }

    public void exit()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
