using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipScene : MonoBehaviour
{
    [SerializeField] private string sceneName;

    // Update is called once per frame
    void Update()
    {
        if (sceneName == string.Empty) return;

        if (Input.GetButton("Fire1"))
            SceneManager.LoadScene(sceneName);
    }
}
