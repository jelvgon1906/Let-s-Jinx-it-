using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    public Image currentHealthBar;
    public static HUDController instance;
    public TextMeshProUGUI scoreText,ammoText;
    public GameObject PauseWindow;
    public int maxAmmo;

    public GameObject textoEstado;
    public GameObject GameOver;
    [SerializeField] private TextMeshProUGUI txtEstado;
    public TMP_InputField inputNombre;
    public Button botonConectar;
    [SerializeField] private GameObject ScoreWindow;

    private void Start()
    {
        instance = this;
        
        
    }
    private void OnEnable()
    {
        currentHealthBar = GameObject.Find("CurrentHealthbar").GetComponent<Image>();
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        //Percentage health bar fill
        currentHealthBar.fillAmount = (float)currentHealth / (float)maxHealth;
    }
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        //Percentage health bar fill
        ammoText.text = currentAmmo.ToString("00") + "/" + maxAmmo.ToString("00");
    }

    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString("0000");
    }

    public void ChangeStatePauseWindow(bool paused)
    {
        PauseWindow.SetActive(paused);
    }

    public void OnClickEnter()
    {
        if (!string.IsNullOrEmpty(inputNombre.text))
        {
            botonConectar.interactable = false;
            ScoreWindow.SetActive(true);
            GameOver.SetActive(false);
            GameManager.instance.UpdateScore();
        }
        else
        {
            textoEstado.SetActive(true);
            txtEstado.text = "Introduzca un nombre correcto para la sala";
        }
    }
}
