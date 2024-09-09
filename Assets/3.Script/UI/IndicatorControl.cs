using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorControl : MonoBehaviour {
    private Quaternion targetRotation;
    private void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * 50f, Space.World);

        // ī�޶� �ٶ󺸵��� �����ϵ�, Y�� ȸ���� ����
        targetRotation = Quaternion.LookRotation(Camera.main.transform.forward);

        // ī�޶��� X��� Z�� ȸ���� �����ϰ�, Y�� ȸ���� ���� (�귿ó�� ��� ȸ��)
        transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetRotation.eulerAngles.z);
    }
}
