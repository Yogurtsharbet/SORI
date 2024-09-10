using DG.Tweening;
using UnityEngine;

public class HalfInvenContainer : MonoBehaviour {
    
    private Vector3 openPos = new Vector3(353f, -24f, 0);
    private Vector3 closePos = new Vector3(353f, -990f, 0);

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenCombineInven() {
        gameObject.SetActive(true);
        FunctionMove(gameObject.transform, openPos);
    }

    public void CloseCombineInven() {
        FunctionMove(gameObject.transform, closePos);
        gameObject.SetActive(false);
    }

    private void FunctionMove(Transform origin, Vector3 destiny) {
        origin.DOLocalMove(destiny, 0.5f, true);
    }
}
