using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;

    private void OnEnable()
    {
        GameManager.ScoreChanged += UpdateScore;
        GameManager.LivesChanged += UpdateLives;
    }

    private void OnDisable()
    {
        GameManager.ScoreChanged -= UpdateScore;
        GameManager.LivesChanged -= UpdateLives;
    }


    private void UpdateScore(int score)
    {
        scoreText.text = "Score: " + score;
    }

    private void UpdateLives(int lives)
    {
        livesText.text = "x" + lives;
    }
}