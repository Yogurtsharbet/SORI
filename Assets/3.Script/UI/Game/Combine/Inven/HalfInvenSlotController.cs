using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 상점, 조합창 - 인벤토리 슬롯 컨트롤러
public class HalfInvenSlotController : CommonInvenSlotController, IPointerClickHandler {
    private HalfInvenManager halfInvenManager;
    private CombineManager combineSlotManager;
    private PlayerInvenController playerInven;

    private void Awake() {
        //TODO: 게임씬으로 분리시  CANVAS 분리 필요
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }

        closeController = GetComponentInChildren<InvenSlotCloseController>();
        halfInvenManager = FindObjectOfType<HalfInvenManager>();
        combineSlotManager = FindObjectOfType<CombineManager>();
        playerInven = FindObjectOfType<PlayerInvenController>();

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
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        //TODO: 클릭했을때 상점이 열려있으면 삭제할건지 dialog
    }

    public void DeleteWord() {
        playerInven.RemoveItemIndex(key);
    }

    public void AddWord(Word word) {
        playerInven.AddNewItem(word);
    }
}
