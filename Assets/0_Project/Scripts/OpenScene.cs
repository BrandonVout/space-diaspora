using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    /// <summary> Open new scene using name of new scene as a string input </summary>
    /// <param name="sceneName"> Scene name as string </param>
    public void Btn_OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}