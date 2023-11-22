using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.SocialPlatforms.Impl;

public class Ball : MonoBehaviour
{
    public Transform launchPoint;
    private ScoreManager scoreManager;
    private bool canScore = true;

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        launchPoint = GameObject.Find("Push Button").transform;
    }

    private IEnumerator ResetScoreCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canScore = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "racket" && canScore)
        {

            canScore = false;
            StartCoroutine(ResetScoreCooldown(3f)); 
        }
    }

    private void OnDestroy()
    {
        float distance = Vector3.Distance(gameObject.transform.position, launchPoint.position);
        scoreManager.SetDistance(distance);
        if (distance >= 6f)
        {
            scoreManager.AddScore(50);
            
        }
        else if (distance >= 3f)
        {
            scoreManager.AddScore(10);
        }
    }
}
