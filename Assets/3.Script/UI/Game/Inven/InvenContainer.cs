using UnityEngine;

// [UI] �κ��丮 - �κ��丮 �����̳� ����
public class InvenContainer : MonoBehaviour {
    private SynthesisManager synthesisManager;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    //TODO: ȿ���ֱ� �κ�~~ �����°� �Ϸη� �ϰ� �ľƾ� �ϸ鼭 ���ƾƾ�~~ �ϰ�~~ ^-^7777
    public void OpenInventory() {
        gameObject.SetActive(true);
    }

    public void CloseInventory() {
        synthesisManager.ResetAllSlot();
        gameObject.SetActive(false);
    }
}
