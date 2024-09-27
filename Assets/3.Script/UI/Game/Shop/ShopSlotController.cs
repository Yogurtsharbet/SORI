using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 상점 - 구매가능한 단어 슬롯 컨트롤러
public class ShopSlotController : MonoBehaviour, IPointerClickHandler {
    private ShopSlotManager shopSlotManager;

    private int key;
    public int Key => key;

    private Word thisWord = null;
    public Word ThisWord => thisWord;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;
    private int price;
    private Text priceText;
    private Image selectSlot;

    private void Awake() {
        shopSlotManager = FindObjectOfType<ShopSlotManager>();
        Text[] texts = GetComponentsInChildren<Text>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("Word")) {
                wordText = txt;
            }
            else if (txt.name.Equals("Price")) {
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
            else if (img.name.Equals("SelectSlot")) {
                selectSlot = img;
            }
        }

        selectSlot.gameObject.SetActive(false);
    }

    public void SetKey(int num) {
        this.key = num;
    }

    private void setWordData(Word word = null) {
        if (word != null) {
            wordText.text = word.Name;
            typeIcon.color = word.TypeColor;
            rankInnerIcon.color = word.RankColor;
            priceText.text = $"x{price}";
            if (word.IsPersist) {
                continueIcon.enabled = true;
            }
            else {
                continueIcon.enabled = false;
            }
        }
        else {
            wordText.text = "";
            typeIcon.color = Color.white;
            rankInnerIcon.color = Color.white;
            continueIcon.enabled = false;
            priceText.text = $"x0";
        }
    }

    public void SetWord(Word word) {
        if (word!=null) {
            price = shopSlotManager.GetWordPrice(word);
        }
        else {
            price = 0;
        }
        thisWord = word;
        setWordData(word);
    }

    public void ActiveSelect(bool yn) {
        selectSlot.gameObject.SetActive(yn);
    }

    public void OnPointerClick(PointerEventData eventData) {
        shopSlotManager.SelectShopItem(key);
    }

    public bool IsExistWord() {
        return thisWord != null ? true : false;
    }

}
