using UnityEngine;

// [UI] 인벤토리 - 슬롯 닫혀있을때 컨트롤러
public class InvenSlotCloseController : MonoBehaviour {

    public void CloseEnable() {
        gameObject.SetActive(true);
    }

    public void OpenDisEnable() {
        gameObject.SetActive(false);
    }
}
