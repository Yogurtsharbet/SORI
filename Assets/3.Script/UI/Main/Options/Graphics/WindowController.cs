using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� - �ɼ� - �������� ���� ��Ʈ�ѷ�
public class WindowController : MonoBehaviour {
    private Text text;
    private bool isFullscreen = false;

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name == "ChangerText") {
                text = txt;
            }
        }
    }

    public void ChangeWindowMode() {
        if (isFullscreen) {
            isFullscreen = true;
            text.text = "��üȭ��";
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else {
            isFullscreen = false;
            text.text = "â���";
            //TODO: �ػ� �����Ѱ����� ��������
            Screen.SetResolution(1280, 720, false);
        }
    }
}
