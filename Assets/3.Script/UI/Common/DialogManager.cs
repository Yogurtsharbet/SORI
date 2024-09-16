using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DialogType {
    SUCCESS,
    WARNING,
    FAIL,
    DEFAULT
}

public class DialogManager :MonoBehaviour {
    private static DialogManager instance = null;

    private Text text;
    private RectTransform rectTransform;
    private Button confirmButton;

    private int isConfirmed = -1;

    public static DialogManager Instance {
        get {
            if (instance == null) {
                return null;
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
        text = GetComponentInChildren<Text>();
        rectTransform = gameObject.GetComponentInChildren<RectTransform>();

        //TODO: CONFIRM 버튼 추가하기
        //confirmButton = GetComponentInChildren<Button>();
        //confirmButton.enabled = false;
    }

    private void Start() {
        CloseDialog();
    }

    public void OpenDefaultDialog(string contents, DialogType type) {
        setDialogText(contents);
        Vector2 resize = getDialogSize(contents);
        rectTransform.sizeDelta = resize;
        text.rectTransform.sizeDelta = resize;
        text.color = getDialogTextColor(type);
        gameObject.SetActive(true);

        StartCoroutine(closeDelay());
    }

    public int GetIsConfirmed() {
        return isConfirmed;
    }

    public void OpenConfirmDialog(string contents, DialogType type) {
        setDialogText(contents);
        Vector2 calSize = getDialogSize(contents);
        Vector2 resize = new Vector2(calSize.x, calSize.y + 100f);
        rectTransform.sizeDelta = resize;
        text.rectTransform.sizeDelta = resize;
        text.color = getDialogTextColor(type);
        gameObject.SetActive(true);
        confirmButton.enabled = true;
    }

    public void ReturnIsConfirm(bool yn) {
        isConfirmed = yn == true ? 1 : 0;
        CloseDialog();
    }

    public void ResetIsConfirm() {
        isConfirmed = -1;
    }

    #region 다이얼로그 설정

    public void CloseDialog() {
        text.text = string.Empty;
        text.color = new Color(0.219f, 0.219f, 0.219f);
        gameObject.SetActive(false);
        confirmButton.enabled = false;
    }

    //사이즈 계산
    private Vector2 getDialogSize(string contents) {
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

    private void setDialogText(string contents) {
        text.text = contents;
    }

    private Color getDialogTextColor(DialogType type) {
        switch (type) {
            case DialogType.SUCCESS:
                return new Color(0.25f, 0.72f, 0.64f);
            case DialogType.WARNING:
                return new Color(1f, 0.388f, 0f);
            case DialogType.FAIL:
                return new Color(0.80f, 0f, 0.119f);
            case DialogType.DEFAULT:
                return new Color(0.219f, 0.219f, 0.219f);
            default:
                return new Color(0.219f, 0.219f, 0.219f);
        }
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
        CloseDialog();
    }
    #endregion
}
