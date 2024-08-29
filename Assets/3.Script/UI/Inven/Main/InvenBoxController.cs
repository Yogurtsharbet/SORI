using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenBoxController : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {
    private Canvas canvas;
    private RectTransform originalParent;
    private Vector2 originalPosition;
    private WordInfoController wordInfoController;

    private Text wordText;
    private Image typeIcon;
    private Image rankIcon;

    private int key;

    private void Awake() {
        wordInfoController = FindObjectOfType<WordInfoController>();
        wordText = GetComponentInChildren<Text>();
        wordText.text = string.Empty;
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image img in images) {
            if (img.name.Equals("TypeIcon")) {
                typeIcon = img;
            }else if (img.name.Equals("RankIcon")) {
                rankIcon = img;
            }
        }
    }

    public void SetKey(int num) {
        this.key = num;
    }

    public void BoxClick() {
        wordInfoController.WordButtonClick(key);
    }

    public void SetWordData(Word word) {
        wordText.text = word.Name;
        typeIcon.color = word.GetTypeColor(word.Type);
        rankIcon.color = word.GetRankColor(word.Rank);
    }

    public void OnPointerClick(PointerEventData eventData) {
        //TODO: 현재 KEY 선택
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData) {
        throw new System.NotImplementedException();
    }

}
