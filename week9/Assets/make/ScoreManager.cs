using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // UI Text 컴포넌트
    public TextMeshProUGUI distText; // UI Text 컴포넌트
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
        scoreText.text = "Score: " + score; // 점수를 업데이트합니다.
        distText.text = "Distance: " + dist;
    }
}
