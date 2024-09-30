using DG.Tweening;
using UnityEngine;

// [UI] 인벤토리 - 인벤토리 컨테이너 관리
public class InvenContainer : MonoBehaviour {
    [SerializeField] private GameObject bgPanel;
    private SynthesisManager synthesisManager;

    private Vector3 openPos = new Vector3(0f, 0f, 0);
    private Vector3 closePos = new Vector3(0f, -1020f, 0);

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenInventory() {
        gameObject.SetActive(true);
        bgPanel.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseInventory() {
        synthesisManager.ResetAllSlot();
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
        bgPanel.SetActive(false);
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 0.5f, true);
    }
}
