using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotController : MonoBehaviour , IPointerClickHandler {

    private int key;
    
    private Word thisWord = null;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;
    private Text priceText;

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach(Text txt in texts) {
            if (txt.name.Equals("Word")) {
                wordText = txt;
            }else if (txt.name.Equals("Price")) {
                priceText = txt;
            }
        }
        
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

    public void SetKey(int num) {
        this.key = num;
    }

    public void SetWordData(Word word) {
        wordText.text = word.Name;
        typeIcon.color = word.TypeColor;
        rankInnerIcon.color = word.RankColor;
        //TODO: 단어에 영구속성 있으면 continue icon enable
    }


    public void OnPointerClick(PointerEventData eventData) {
        //SelectSlot()
    }
}
