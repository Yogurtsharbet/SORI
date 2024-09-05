using UnityEngine;
using UnityEngine.UI;

public class SynthesisSlotController : MonoBehaviour {
    private int key;

    private Word thisWord;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;

    private void Awake() {
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

    public void ExistWord() {
        wordText.enabled = true;
        typeIcon.enabled = true;
        rankInnerIcon.enabled = true;
        rankOutIcon.enabled = true;
        continueIcon.enabled = true;
    }

    public void NotExistWord() {
        wordText.enabled = false;
        typeIcon.enabled = false;
        rankInnerIcon.enabled = false;
        rankOutIcon.enabled = false;
        continueIcon.enabled = false;
    }

    public Word GetSlotWord() {
        return thisWord;
    }

    public void RemoveSlotWord() {
        thisWord = null;
    }

    public void SetSlotWord(Word word) {
        thisWord = word;
    }

    public bool GetWordExist() {
        if (thisWord != null) {
            return true;
        }
        else {
            return false;
        }
    }
}
