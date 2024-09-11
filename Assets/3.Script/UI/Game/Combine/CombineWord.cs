using UnityEngine;
using UnityEngine.UI;

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
        //TODO: 단어에 영구속성 있으면 continue icon enable
    }

    public void CloseSlot() {
        CheckWordExist();
    }

    public void OpenSlot() {
        CheckWordExist();
    }
}
