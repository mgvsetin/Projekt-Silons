using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instace;

    public Text scoreText;
    public Text highScoreText;

    public static int pointsToAdd = 50;
    public static int suspiciousPoints = 1;
    public static int alarmedPoints = 2;
    public static int ChasingPoints = 5;

    public static int score;
    public static int highScore;

    public void Awake()
    {
        instace = this;
    }

    public void Start()
    {
        if(scoreText != null || highScoreText != null)
        {
            highScore = PlayerPrefs.GetInt("highScore", 0);
            scoreText.text = "Vaše skóre: " + score.ToString();
            highScoreText.text = "Nejvyšší skóre: " + highScore.ToString();
        }
    }

    public void AddPoints()
    {
        score += pointsToAdd;
        if(score > highScore)
        {
            PlayerPrefs.SetInt("highScore", score);
        }
        transform.GetComponent<AudioSource>().Play();
    }
    public void RemoveSuspiciousPoints()
    {
        score -= suspiciousPoints;
    }

    public void RemoveAlarmedPoints()
    {
        score -= alarmedPoints;
    }

    public void RemoveChasingPoints()
    {
        score -= ChasingPoints;
    }
}
