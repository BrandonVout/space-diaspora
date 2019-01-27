using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenScene : MonoBehaviour
{
    [SerializeField] private float pauseTime = 0.5f;
    
    /// <summary> Open new scene using name of new scene as a string input </summary>
    /// <param name="sceneName"> Scene name as string </param>
    public void Btn_OpenScene(string sceneName)
    {
        StartCoroutine(Pause(sceneName));
    }

    private IEnumerator Pause(string sceneName)
    {
        yield return new WaitForSeconds(pauseTime);
        SceneManager.LoadScene(sceneName);
    }
}