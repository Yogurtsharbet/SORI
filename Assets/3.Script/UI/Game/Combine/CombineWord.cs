using UnityEngine;
using UnityEngine.UI;

// [UI] 조합 - 조합창 단어 슬롯 공통
public class CombineWord : MonoBehaviour {

    protected Word thisWord;
    public Word SlotWord => thisWord;

    protected Text wordText;
    protected Image typeIcon;
    protected Image rankOutIcon;
    protected Image rankInnerIcon;
    protected Image continueIcon;
    protected Image closeImage;

    public void CheckWordExist() {
        if (thisWord != null) {
            ExistWord();
            SetWordData(thisWord);
            closeImage.gameObject.SetActive(false);
        }
        else {
            NotExistWord();
            closeImage.gameObject.SetActive(true);
        }
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

    public void SetSlotWord(Word word) {
        thisWord = word;
        CheckWordExist();
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

    public void CloseSlot() {
        CheckWordExist();
    }

    public void OpenSlot() {
        CheckWordExist();
    }
}
