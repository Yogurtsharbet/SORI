using UnityEngine;

// [UI] �κ��丮 - ���� ���������� ��Ʈ�ѷ�
public class InvenSlotCloseController : MonoBehaviour {

    public void CloseEnable() {
        gameObject.SetActive(true);
    }

    public void OpenDisEnable() {
        gameObject.SetActive(false);
    }
}
