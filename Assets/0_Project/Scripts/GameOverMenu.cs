using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FadeText))]
public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private string gameOver;

    private void Start()
    {
        GetComponent<FadeText>().showsOver += End;
    }

    private void End(object sender, EventArgs args)
    {
        if (gameOver == string.Empty) return;
        SceneManager.LoadScene(gameOver);
    }
}