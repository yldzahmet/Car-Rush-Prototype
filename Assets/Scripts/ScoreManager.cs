using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    FinishFlag finishFlag;
    internal static bool updateScore = false;
    internal static int score = 400;
    public Text scoreText;

    private void Start()
    {
        finishFlag = GetComponent<FinishFlag>();
    }

    public void EditScore(int number)
    {
        score += number * 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(scoreText)
            scoreText.text = string.Concat("Score: ", score.ToString());

        if(score <= 0 && updateScore)
        {
            finishFlag.Failed();
        }
    }
}
