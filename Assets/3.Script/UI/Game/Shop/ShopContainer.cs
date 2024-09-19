using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [UI] 상점 - 상점 UI 컨테이너 관리
public class ShopContainer : MonoBehaviour {

    private Vector3 openPos = new Vector3(-360f, -68f, 0);
    private Vector3 closePos = new Vector3(-360f, -970f, 0);

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenShopContainer() {
        gameObject.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseShopContainer() {
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 0.5f, true);
    }
}
