using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 조합 - 조합창 단어 슬롯
public class CombineSlotController : CombineWord, IPointerClickHandler {
    private HalfInvenContainer halfInvenContainer;

    private void Awake() {
        wordText = GetComponentInChildren<Text>();
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();
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
            }else if (img.name.Equals("CloseSlot")) {
                closeImage = img;
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        halfInvenContainer.OpenCombineInven();
    }
}
