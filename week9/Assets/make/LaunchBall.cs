using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaunchBall : MonoBehaviour
{
    [SerializeField] private GameObject ball; // 발사할 공 프리팹
    [SerializeField] private float force = 3.0f; // 공을 발사하는 힘
    [SerializeField] private float lifeTime = 10.0f; // 공의 수명
    [SerializeField] private TextMeshProUGUI scoreText; // 점수를 표시하는 UI Text
    [SerializeField] private TextMeshProUGUI distText; // 점수를 표시하는 UI Text
    [SerializeField] private Transform launchPoint;


    public void DrawBall()
    {
        if (ball != null)
        {
            GameObject gameObject = Instantiate(ball); // 공 생성
            gameObject.transform.position = launchPoint.position + new Vector3(0, 0.0055f, 0); // 공의 위치 설정
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>(); // 공의 리지드바디 컴포넌트 가져오기
            rigidbody.AddForce(Vector3.up * force); // 공에 힘을 가하기
            Destroy(gameObject, lifeTime); // 일정 시간 후 공 제거
        }
    }

}
