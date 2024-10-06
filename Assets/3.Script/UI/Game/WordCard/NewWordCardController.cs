using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewWordCardController : MonoBehaviour, IPointerClickHandler {
    private WordCardSelectContainer wordCardSelectContainer;
    private CardSelectEffect cardSelectEffect;
    private CardAppearEffect cardAppearEffect;

    private Text wordText;
    private Image typeIcon;
    private Image rankOutIcon;
    private Image rankInnerIcon;
    private Image continueIcon;

    private Word thisWord = null;
    public Word ThisWord => thisWord;

    private int key = -1;

    public int Key => key;

    private void Awake() {
        GetCardComponent();
    }

    private void GetCardComponent() {
        wordCardSelectContainer = FindObjectOfType<WordCardSelectContainer>();
        cardSelectEffect = GetComponentInChildren<CardSelectEffect>();
        cardAppearEffect = GetComponentInChildren<CardAppearEffect>();
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

    #region wordCard DataSetting

    public void SetKey(int num) {
        key = num;
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
    #endregion

    public void SetCardPositionSetting(Vector3 pos) {
        cardAppearEffect.SetPostion(pos);
        cardSelectEffect.SetSelectInit(new Vector3(pos.x, pos.y + 70f, pos.z));
    }

    public void StartAppear() {
        cardAppearEffect.ShowCard();
    }

    public void OnPointerClick(PointerEventData eventData) {
        wordCardSelectContainer.SelectNewWord(thisWord);
    }
}
