using UnityEngine;

public class InteractSquare : MonoBehaviour {

    public void OpenSquare(Vector3 pos) {
        gameObject.transform.position = pos;
        gameObject.SetActive(true);
    }

    public void CloseSquare() {
        gameObject.SetActive(false);
    }

    public void UpdatePosition(Vector3 pos) {
        gameObject.transform.position = pos;
    }
}
