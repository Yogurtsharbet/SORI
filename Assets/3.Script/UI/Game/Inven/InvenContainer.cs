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

    public void OpenInventory() {
        gameObject.SetActive(true);
    }

    public void CloseInventory() {
        synthesisManager.ResetAllSlot();
        gameObject.SetActive(false);
    }
}
