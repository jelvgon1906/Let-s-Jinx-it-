using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;

    public static GameManager instance;
    private bool gameEnd;
    public bool gamePaused;
    [SerializeField] private GameObject hands;


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
        if (Input.GetButtonUp("Cancel") && !gameEnd)
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
    public void EndGame()
    {
        gameEnd = true;
        gamePaused = !gamePaused;
        //if hamePaused freeze game, else continue normal
        Time.timeScale = (gamePaused) ? 0.0f : 1f;
        //lock & unlock cursor
        Cursor.lockState = (gamePaused) ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void UpdateScore()
    {
        ScoreUi.scoreUi.ScoreUpdate(score);
    }

    public void SpawnEnemies()
    {
        Debug.Log("uwu");
        FindObjectOfType<SpawnEnemies>().enabled = true;
        Destroy(FindObjectOfType<CinemachineVirtualCamera>().gameObject);
        hands.SetActive(true);
    }

    
}
