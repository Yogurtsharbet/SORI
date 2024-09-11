using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] �κ��丮 - ���� ���� ��Ʈ�ѷ�
public class CommonInvenSlotController : MonoBehaviour{
    protected InvenSlotCloseController closeController;

    protected int key;

    protected Word thisWord = null;
    protected Text wordText;
    protected Image typeIcon;
    protected Image rankOutIcon;
    protected Image rankInnerIcon;
    protected Image continueIcon;

    protected Canvas canvas;
    protected RectTransform originalParent;
    protected Vector2 originalPosition;

    public void SetKey(int num) {
        this.key = num;
    }

    public void CheckWordExist() {
        if (thisWord != null) {
            ExistWord();
            SetWordData(thisWord);
        }
        else {
            NotExistWord();
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

    public void SetWordData(Word word) {
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
        closeController.CloseEnable();
        CheckWordExist();
    }

    public void OpenSlot() {
        closeController.OpenDisEnable();
        CheckWordExist();
    }
}
