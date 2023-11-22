using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaunchBall : MonoBehaviour
{
    [SerializeField] private GameObject ball; // �߻��� �� ������
    [SerializeField] private float force = 3.0f; // ���� �߻��ϴ� ��
    [SerializeField] private float lifeTime = 10.0f; // ���� ����
    [SerializeField] private TextMeshProUGUI scoreText; // ������ ǥ���ϴ� UI Text
    [SerializeField] private TextMeshProUGUI distText; // ������ ǥ���ϴ� UI Text
    [SerializeField] private Transform launchPoint;


    public void DrawBall()
    {
        if (ball != null)
        {
            GameObject gameObject = Instantiate(ball); // �� ����
            gameObject.transform.position = launchPoint.position + new Vector3(0, 0.0055f, 0); // ���� ��ġ ����
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>(); // ���� ������ٵ� ������Ʈ ��������
            rigidbody.AddForce(Vector3.up * force); // ���� ���� ���ϱ�
            Destroy(gameObject, lifeTime); // ���� �ð� �� �� ����
        }
    }

}
