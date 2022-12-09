using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreUi : MonoBehaviour
{
    public RowUi rowUi;
    public ScoreManager scoreManager;
    public string inputName;
    public int inputScore;
    public TMP_InputField inputNombre;
    public static ScoreUi scoreUi;

    void Start()
    {
        scoreUi = this;
    }

    public void ScoreUpdate(int score)
    {
        inputName = inputNombre.text;
        inputScore = score;
        scoreManager.AddScore(new Score(name: inputName, score: inputScore));


        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUi>();
            row.rank.text = (i + 1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].score.ToString();
        }
    }
}

