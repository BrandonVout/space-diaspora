using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerHealth))]
public class GameOver : MonoBehaviour
{
    [SerializeField] private string _gameOver;

    private void Start()
    {
        GetComponent<PlayerHealth>().Dead += Dead;
    }

    private void Dead(object sender, EventArgs args)
    {
        if(_gameOver == string.Empty) return;
        SceneManager.LoadScene(_gameOver);
    }
}
