using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewWordCardController : MonoBehaviour, IPointerClickHandler {
    private WordCardSelectContainer wordCardSelectContainer;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;

    private Word thisWord = null;
    public Word ThisWord => thisWord;

    public void GetCardComponent() {
        wordCardSelectContainer = FindObjectOfType<WordCardSelectContainer>();
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
    }

    public void SetWordData(Word wordData) {
        thisWord = wordData;
        CheckWordData();
    }

    private void CheckWordData() {
        if (thisWord != null) {
            wordText.text = thisWord.Name;
            typeIcon.color = thisWord.TypeColor;
            rankInnerIcon.color = thisWord.RankColor;
            if (thisWord.IsPersist) {
                continueIcon.enabled = true;
            }
            else {
                continueIcon.enabled = false;
            }
        }
        else {
            wordText.text = string.Empty;
            typeIcon.color = Color.white;
            rankInnerIcon.color = Color.white;
            continueIcon.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        //TODO: 보여주는 선택 단어 삭제, 초기화, SELECT CONTAINER 숨김
        wordCardSelectContainer.SelectNewWord(thisWord);
    }
}
