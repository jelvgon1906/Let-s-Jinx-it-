using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;

public class GameManager : MonoBehaviour
{
    public int score;

    public static GameManager instance;
    private bool gameEnd;
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
        UnityEngine.Cursor.lockState = (gamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

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
        /*GameObject.Find("CameraPlayer").GetComponent<Transform>()*/
        Destroy(FindObjectOfType<CinemachineVirtualCamera>().gameObject);
        GameObject.Find("CameraPlayer").GetComponent<Transform>().SetLocalPositionAndRotation(new Vector3(0, 0.4861f, 0.037f), new Quaternion(0, 0, 0, 0));
    }

    
}
