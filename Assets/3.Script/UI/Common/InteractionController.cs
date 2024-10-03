using UnityEngine;

public class InteractionController : MonoBehaviour {
    private InteractBubble bubble;
    private InteractSquare square;

    private void Awake() {
        bubble = GetComponentInChildren<InteractBubble>();
        square = GetComponentInChildren<InteractSquare>();
    }

    private void Start() {
        bubble.gameObject.SetActive(false);
        square.gameObject.SetActive(false);
    }

    public void ShowBubble(string content) {
        bubble.OpenBubble(content);
    }

    public void HideBubble() {
        bubble.CloseBubble();
    }

    public void ShowSqure(Vector3 pos) {
        square.OpenSquare(pos);
    }

    public void HideSqure() {
        square.CloseSquare();
    }
}
