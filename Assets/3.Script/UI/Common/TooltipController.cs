using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipController : MonoBehaviour {
    private Text text;
    private RectTransform rectTransform;

    private void Awake() {
        text = GetComponentInChildren<Text>();
        rectTransform = gameObject.GetComponentInChildren<RectTransform>();
    }

    private void Start() {
        CloseTooltip();
    }

    public void SetTooltipText(string contents) {
        text.text = contents;
    }

    public void OpenTooltip(PointerEventData eventData) {
        gameObject.transform.position = new Vector3(eventData.position.x, eventData.position.y - 80f, 0f);
        string contents = GetTooltipContents(eventData.pointerEnter.name);
        SetTooltipText(contents);

        Vector2 resize = getTooltipSize(contents);
        rectTransform.sizeDelta = resize;
        text.rectTransform.sizeDelta = resize;

        gameObject.SetActive(true);
    }

    public void CloseTooltip() {
        gameObject.SetActive(false);
    }

    private Vector2 getTooltipSize(string contents) {
        (int, int) sentenceCountes = sentenceCount(contents);

        float width;
        if (sentenceCountes.Item1 < 6) {
            width = 180f;
        }
        else {
            width = 180f + (sentenceCountes.Item1 - 5) * 20f;
        }

        float height;
        if (sentenceCountes.Item2 < 2) {
            height = 110f;
        }
        else {
            height = 110f + (sentenceCountes.Item2 - 1) * 24f;
        }

        return new Vector2(width, height);
    }

    private (int, int) sentenceCount(string contents) {
        string[] sentences = contents.Split('\n');
        int maxWidth = 0;
        for (int i = 0; i < sentences.Length; i++) {
            if (maxWidth < sentences[i].Length) {
                maxWidth = sentences[i].Length;
            }
        }
        return (maxWidth, sentences.Length);
    }

    private string GetTooltipContents(string name) {
        if (name.Equals("SynthesisTooltip")) {
            return "소유중인 단어 3개를 소모하여\n새로운 단어를 얻을 수 있습니다.";
        }
        else {
            return "";
        }
    }
}
