using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour {
    public static FadeControl Instance;
    private Image fadeScreen;
    private Color screenColor;

    [Range(1f, 5f)]
    [SerializeField] private float FadeTime = 1f;

    private void Awake() {
        Instance = this;
        fadeScreen = GetComponent<Image>();
    }

    private void OnEnable() {
        FadeIn();
    }

    private void StopFade() {
        StopCoroutine(FadeIn_Co());
        StopCoroutine(FadeOut_Co());
    }

    public void FadeIn() {
        StopFade();
        screenColor = fadeScreen.color;
        screenColor.a = 1f;
        fadeScreen.color = screenColor;
        StartCoroutine(FadeIn_Co());
    }

    private IEnumerator FadeIn_Co() {
        float fadeStart = Time.time;
        do {
            screenColor = fadeScreen.color;
            screenColor.a = 1f - Mathf.Pow(Mathf.InverseLerp(0, FadeTime, Time.time - fadeStart), 2);
            if (screenColor.a < 0.01f) screenColor.a = 0;
            fadeScreen.color = screenColor;
            yield return null;
        } while (screenColor.a > 0);
    }

    public void FadeOut() {
        StopFade();
        screenColor = fadeScreen.color;
        screenColor.a = 0f;
        fadeScreen.color = screenColor;
        StartCoroutine(FadeOut_Co());
    }

    private IEnumerator FadeOut_Co() {
        float fadeStart = Time.time;
        do {
            screenColor = fadeScreen.color;
            screenColor.a = 0f + Mathf.Pow(Mathf.InverseLerp(0, FadeTime, Time.time - fadeStart), 2);
            if (screenColor.a > 0.99f) screenColor.a = 1f;
            fadeScreen.color = screenColor;
            yield return null;
        } while (screenColor.a < 1f);
    }
}

