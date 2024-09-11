using UnityEngine.UI;

// [UI] 문장 목록 - 문장 목록에 띄워줄 단어 슬롯
public class SentenceWordSlot : CombineWord {
    private void Awake() {
        wordText = GetComponentInChildren<Text>();
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
            else if (img.name.Equals("CloseSlot")) {
                closeImage = img;
            }
        }
    }
}
