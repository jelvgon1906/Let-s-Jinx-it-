using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;

    public static GameManager instance;

    public bool gamePaused;


    private void Awake()
    {
        instance = this;
    }

    public void UpdateScore(int points)
    {
        score += points;
        HUDController.instance.UpdateScore(score);
    }
    public void UpdateButton(bool paused)
    {
        UpdateGamePause();
    }

    private void Update()
    {
        if (Input.GetButtonUp("Cancel") )
        {
            UpdateGamePause();
            
        }
    }

    private void UpdateGamePause()
    {
        gamePaused = !gamePaused;
        //if hamePaused freeze game, else continue normal
        Time.timeScale = (gamePaused) ? 0.0f : 1f;
        //lock & unlock cursor
        Cursor.lockState = (gamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

        HUDController.instance.ChangeStatePauseWindow(gamePaused);
    }
}
