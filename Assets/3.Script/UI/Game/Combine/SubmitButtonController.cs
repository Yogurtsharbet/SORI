using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 조합 - 확정 버튼 관리
public class SubmitButtonController : MonoBehaviour {
    private Image buttonImage;
    private Text buttonText;
    private Outline outline;

    private void Awake() {
        buttonImage = GetComponentInChildren<Image>();
        buttonText = GetComponentInChildren<Text>();
        outline = buttonText.gameObject.GetComponentInChildren<Outline>();
    }

    public void ButtonToSubmit() {
        buttonImage.color = new Color(0.494f, 0.788f, 0.729f);
        buttonText.text = "조립!";
        outline.effectColor = new Color(0.329f, 0.678f, 0.713f);
    }

    public void ButtonToRemove() {
        buttonImage.color = new Color(0.631f, 0.133f, 0.2f);
        buttonText.text = "비우기";
        outline.effectColor = new Color(0.819f, 0.305f, 0.376f);
    }
}
