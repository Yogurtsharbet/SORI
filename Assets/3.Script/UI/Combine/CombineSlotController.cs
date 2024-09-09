using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombineSlotController : MonoBehaviour, IPointerClickHandler {
    private CombineInvenContainer combineInvenContainer;

    protected Word thisWord;
    protected Text wordText;
    protected Image typeIcon;
    protected Image rankOutIcon;
    protected Image rankInnerIcon;
    protected Image continueIcon;

    private void Awake() {
        wordText = GetComponentInChildren<Text>();
        combineInvenContainer = FindObjectOfType<CombineInvenContainer>();
        wordText.text = string.Empty;
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
        //TODO: 단어에 영구속성 있으면 continue icon enable
    }

    public void CloseSlot() {
        CheckWordExist();
    }

    public void OpenSlot() {
        CheckWordExist();
    }

    public void OnPointerClick(PointerEventData eventData) {
        combineInvenContainer.OpenCombineInven();
    }
}
