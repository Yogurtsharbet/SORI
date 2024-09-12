using UnityEngine;

// [UI] 인벤토리 - 인벤토리 컨테이너 관리
public class InvenContainer : MonoBehaviour {
    private SynthesisManager synthesisManager;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    //TODO: 효과넣기 인벤~~ 열리는거 뾰로롱 하고 파아앗 하면서 샤아아아~~ 하게~~ ^-^7777
    public void OpenInventory() {
        gameObject.SetActive(true);
    }

    public void CloseInventory() {
        synthesisManager.ResetAllSlot();
        gameObject.SetActive(false);
    }
}
