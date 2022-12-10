using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager1 : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("InfiniteRoom");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
    public void OnClickMenu()
    {
        SceneManager.LoadScene("Start");
    }
}
