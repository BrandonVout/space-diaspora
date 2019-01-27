using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class GameOver : MonoBehaviour
{
    [SerializeField] private string gameOver;

    private void Start()
    {
        GetComponent<PlayerHealth>().dead += Dead;
    }

    private void Dead(object sender, EventArgs args)
    {
        if (gameOver == string.Empty) return;
        SceneManager.LoadScene(gameOver);
    }
}