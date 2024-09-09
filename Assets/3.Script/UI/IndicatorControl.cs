using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorControl : MonoBehaviour {
    private Quaternion targetRotation;
    private void Update() {
        transform.Rotate(Vector3.up * Time.deltaTime * 50f, Space.World);

        // 카메라를 바라보도록 설정하되, Y축 회전은 유지
        targetRotation = Quaternion.LookRotation(Camera.main.transform.forward);

        // 카메라의 X축과 Z축 회전만 적용하고, Y축 회전은 유지 (룰렛처럼 계속 회전)
        transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetRotation.eulerAngles.z);
    }
}
