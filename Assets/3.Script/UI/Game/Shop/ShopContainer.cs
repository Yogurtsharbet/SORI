using DG.Tweening;
using UnityEngine;

// [UI] 상점 - 상점 UI 컨테이너 관리
public class ShopContainer : MonoBehaviour {
    private HalfInvenContainer halfInvenContainer;
    private Vector3 openPos = new Vector3(-360f, -68f, 0);
    private Vector3 closePos = new Vector3(-360f, -970f, 0);

    private void Awake() {
        halfInvenContainer = FindObjectOfType<HalfInvenContainer>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    // gameManager.ChangeState(GameState.Shop);
    // TODO: 상점 열게 하는 무언가가 있을 때 호출
    public void OpenShopContainer() {
        halfInvenContainer.OpenHalfInven();
        gameObject.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseShopContainer() {
        halfInvenContainer.CloseHalfInven();
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 0.5f, true);
    }
}
