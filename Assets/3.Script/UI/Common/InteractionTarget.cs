using UnityEngine;

public class InteractionTarget : MonoBehaviour {
    private Camera thisCamera;
    private InteractionController interactUI;

    [SerializeField] private InteractType type;
    [SerializeField, TextArea(3, 10)] private string content;

    private bool isTargetBound = false;
    public bool IsTargetBound => isTargetBound;

    private void Awake() {
        interactUI = FindObjectOfType<InteractionController>();
        thisCamera = Camera.main;
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (!isTargetBound && !interactUI.IsOpenSquare) {
                isTargetBound = true;
                interactUI.ShowSqure(GetTargetPosition(), type);
            }

            if (isTargetBound && interactUI.IsOpenSquare) {
                interactUI.UpdateSquarePosition(GetTargetPosition());
            }
        }
    }

    private Vector3 GetTargetPosition() {
        Vector3 screenPos = thisCamera.WorldToScreenPoint(transform.position);
        return screenPos;
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            interactUI.HideSqure();
            isTargetBound = false;
        }
    }
}
