using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InteractBubble : MonoBehaviour {
    private Text text;
    private RectTransform rectTransform;

    private void Awake() {
        text = GetComponentInChildren<Text>();
        rectTransform = gameObject.GetComponentInChildren<RectTransform>();
    }

    public void CloseBubble() {
        StartCoroutine(closeDelay());
    }

    public void OpenBubble(string contents) {
        setBubbleText(contents);
        Vector2 calSize = getBubbleSize(contents);
        Vector2 containerResize = new Vector2(calSize.x, calSize.y + 70f);
        Vector2 textResize = new Vector2(calSize.x, calSize.y);
        rectTransform.sizeDelta = containerResize;
        text.rectTransform.sizeDelta = textResize;
        gameObject.SetActive(true);
    }

    private Vector2 getBubbleSize(string contents) {
        (int, int) sentenceCountes = sentenceCount(contents);

        float width;
        if (sentenceCountes.Item1 < 6) {
            width = 190f;
        }
        else {
            width = 180f + (sentenceCountes.Item1 - 5) * 25f;
        }

        float height;
        if (sentenceCountes.Item2 < 2) {
            height = 120f;
        }
        else {
            height = 110f + (sentenceCountes.Item2 - 1) * 30f;
        }

        return new Vector2(width, height);
    }

    private void setBubbleText(string contents) {
        text.text = contents;
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

    private IEnumerator closeDelay() {
        yield return new WaitForSeconds(1.5f);
        text.text = string.Empty;
        gameObject.SetActive(false);
    }


}
