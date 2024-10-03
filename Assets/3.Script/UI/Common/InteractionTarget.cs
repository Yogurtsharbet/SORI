using UnityEngine;

public class InteractionTarget : MonoBehaviour {
    private Camera thisCamera;
    private InteractionController interactUI;

    [SerializeField] private InteractType type;
    [SerializeField, TextArea(3, 10)] private string content;

    private void Awake() {
        interactUI = FindObjectOfType<InteractionController>();
        thisCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            interactUI.ShowSqure(GetTargetPosition(), type);
        }
    }

    private Vector3 GetTargetPosition() {
        Vector3 screenPos = thisCamera.WorldToScreenPoint(transform.position);
        return screenPos;
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            interactUI.HideSqure();
        }
    }
}
