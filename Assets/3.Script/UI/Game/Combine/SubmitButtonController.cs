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
        buttonImage.color = new Color(126f, 201f, 186f);
        buttonText.text = "확정하기";
        outline.effectColor = new Color(84f, 173f, 182f);
    }

    public void ButtonToRemove() {
        buttonImage.color = new Color(161f, 34f, 51f);
        buttonText.text = "삭제하기";
        outline.effectColor = new Color(209f, 78f, 96f);
    }
}
