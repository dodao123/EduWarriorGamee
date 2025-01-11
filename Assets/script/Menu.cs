using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(2);
    }
    public void Setting()
    {
        SceneManager.LoadScene(1);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(3);
    }
}
