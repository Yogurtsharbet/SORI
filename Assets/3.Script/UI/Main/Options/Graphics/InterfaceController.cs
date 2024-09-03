using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� - �ɼ� - �������̽� ���� ��Ʈ�ѷ�
public class InterfaceController : MonoBehaviour {
    private Slider slider;
    private CanvasScaler[] canvasScaler;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
        canvasScaler = GetComponentsInParent<CanvasScaler>();
    }

    private void Start() {
        slider.onValueChanged.AddListener(delegate {
            SetUIScale(slider.value);
        });
        slider.value = canvasScaler[0].scaleFactor;

    }

    public void SetUIScale(float scaleFactor) {
        if (canvasScaler[0].uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize) {
            for (int i = 0; i < canvasScaler.Length; i++) {
                canvasScaler[i].matchWidthOrHeight = scaleFactor;
            }
        }
        //TODO: �ػ� scale ĳ���ؼ� ���� �信���� scale �����ؾ���
    }
}
