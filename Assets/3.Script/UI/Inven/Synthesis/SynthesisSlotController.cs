using UnityEngine;
using UnityEngine.UI;

public class SynthesisSlotController : MonoBehaviour {
    private int key;

    private int originInvenIndex = -1;
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

    public int GetWordOriginInvenIndex() {
        return originInvenIndex;
    }

    public void RemoveSlotWord() {
        thisWord = null;
        originInvenIndex = -1;
        NotExistWord();
    }

    //재조합 슬롯에 단어 추가 후 인벤에서 단어 삭제
    public void SetSlotWord(Word word, int invenIndex) {
        thisWord = word;
        originInvenIndex = invenIndex;
        ExistWord();
        SetWordData(thisWord);
    }

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

    public void SetWordData(Word word) {
        wordText.text = word.Name;
        typeIcon.color = word.TypeColor;
        rankInnerIcon.color = word.RankColor;
        //TODO: 단어에 영구속성 있으면 continue icon enable
    }
}
