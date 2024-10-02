using DG.Tweening;
using UnityEngine;

// [UI] 조합 - 조합창 컨테이너
public class CombineContainer : MonoBehaviour {
    [SerializeField] private GameObject bgPanel;
    private CombineManager combineManager;
    private HalfInvenManager halfInvenManager;

    private Vector3 openPos = new Vector3(-246f, 27f, 0);
    private Vector3 closePos = new Vector3(-246f, -864f, 0);

    private void Awake() {
        combineManager = FindObjectOfType<CombineManager>();
        halfInvenManager = FindObjectOfType<HalfInvenManager>();
    }

    private void Start() {
        bgPanel.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenCombineField() {
        gameObject.SetActive(true);
        bgPanel.gameObject.SetActive(true);
        halfInvenManager.SetCombineMode(true);
        halfInvenManager.OpenInven();
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseCombineField(bool backToTopview = false) {
        bgPanel.gameObject.SetActive(false);
        if (backToTopview)
            FindObjectOfType<GameManager>().gameState.OnCancel();
        else {
            combineManager.CloseCombineSlot();
            FunctionMove(gameObject.transform, closePos);
            halfInvenManager.SetCombineMode(false);
            halfInvenManager.CloseInven();
            gameObject.SetActive(false);
        }
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 1f, true);
    }
}
