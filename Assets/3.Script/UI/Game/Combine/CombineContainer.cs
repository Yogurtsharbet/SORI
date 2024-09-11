using DG.Tweening;
using UnityEngine;

// [UI] 조합 - 조합창 컨테이너
public class CombineContainer : MonoBehaviour {
    private CombineSlotManager combineSlotManager;
    private HalfInvenManager halfInvenManager;

    private Vector3 openPos = new Vector3(-246f, 27f, 0);
    private Vector3 closePos = new Vector3(-246f, -864f, 0);

    private void Awake() {
        combineSlotManager = FindObjectOfType<CombineSlotManager>();
        halfInvenManager = FindObjectOfType<HalfInvenManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenCombineField() {
        gameObject.SetActive(true);
        halfInvenManager.SetCombineMode(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseCombineField() {
        FunctionMove(gameObject.transform, closePos);
        combineSlotManager.CloseCombineSlot();
        halfInvenManager.SetCombineMode(false);
        halfInvenManager.CloseInven();
        gameObject.SetActive(false);
    }

    public void OpenCombineSlot(int key, Sentence sentence) {
        combineSlotManager.OpenCombineSlot(key, sentence);
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 1f, true);
    }
}
