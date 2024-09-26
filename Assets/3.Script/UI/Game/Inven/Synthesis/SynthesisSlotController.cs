using UnityEngine;
using UnityEngine.UI;

// [UI] 합성 - 합성창 단어 슬롯 컨트롤
public class SynthesisSlotController : MonoBehaviour {
    private SynthesisManager synthesisManager;

    private int key;

    private Word thisWord;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
        wordText = GetComponentInChildren<Text>();
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image img in images) {
            if (img.name.Equals("Type")) {
                typeIcon = img;
            }
            else if (img.name.Equals("Rank")) {
                rankOutIcon = img;
            }
            else if (img.name.Equals("RankColor")) {
                rankInnerIcon = img;
            }
            else if (img.name.Equals("Continue")) {
                continueIcon = img;
            }
        }

        if (gameObject.name.Contains("0")) {
            key = 0;
        }
        else if (gameObject.name.Contains("1")) {
            key = 1;
        }
        else if (gameObject.name.Contains("2")) {
            key = 2;
        }
        else {
            key = 3;
        }
    }

    private void Start() {
        NotExistWord();
    }

    private void ExistWord() {
        wordText.enabled = true;
        typeIcon.enabled = true;
        rankInnerIcon.enabled = true;
        rankOutIcon.enabled = true;
        continueIcon.enabled = true;
    }

    private void NotExistWord() {
        wordText.enabled = false;
        typeIcon.enabled = false;
        rankInnerIcon.enabled = false;
        rankOutIcon.enabled = false;
        continueIcon.enabled = false;

        wordText.text = string.Empty;
        typeIcon.color = new Color(1f, 1f, 1f);
        rankInnerIcon.color = new Color(1f, 1f, 1f);
    }

    public Word GetSlotWord() {
        return thisWord;
    }

    public void RemoveSlotWord() {
        thisWord = null;
        NotExistWord();
    }

    /// <summary>
    /// 슬롯에 단어 추가
    /// </summary>
    /// <param name="word">Word</param>
    public void SetSlotWord(Word word) {
        thisWord = word;
        ExistWord();
        SetWordData(thisWord);
    }

    public bool GetWordExist() {
        if (thisWord != null) {
            return true;
        }
        else {
            return false;
        }
    }

    private void SetWordData(Word word) {
        wordText.text = word.Name;
        typeIcon.color = word.TypeColor;
        rankInnerIcon.color = word.RankColor;
        if (word.IsPersist) {
            continueIcon.enabled = true;
        }
        else {
            continueIcon.enabled = false;
        }
    }

    public void AddWord(int invenKey) {
        synthesisManager.WordSwitchingToSynthesis(invenKey, key);
    }
}
