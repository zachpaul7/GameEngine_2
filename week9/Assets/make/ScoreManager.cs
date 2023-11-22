using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // UI Text ������Ʈ
    public TextMeshProUGUI distText; // UI Text ������Ʈ
    private int score = 0;
    private float dist = 0;

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public void SetDistance(float distance)
    {
        dist = distance;
    }

    void Update()
    {
        scoreText.text = "Score: " + score; // ������ ������Ʈ�մϴ�.
        distText.text = "Distance: " + dist;
    }
}
