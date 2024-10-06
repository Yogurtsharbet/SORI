using DG.Tweening;
using UnityEngine;

// [UI] 상점 - 상점 UI 컨테이너 관리
public class ShopContainer : MonoBehaviour {
    [SerializeField] private GameObject bgPanel;
    private HalfInvenContainer halfInvenContainer;

    private Vector3 openPos = new Vector3(-360f, -68f, 0);
    private Vector3 closePos = new Vector3(-360f, -970f, 0);

    private void Awake() {
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenShopContainer() {
        bgPanel.SetActive(true);
        halfInvenContainer.OpenHalfInven();
        gameObject.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseShopContainer() {
        bgPanel.SetActive(false);
        halfInvenContainer.CloseHalfInven();
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 0.5f, true);
    }
}
