using DG.Tweening.Core.Easing;
using UnityEngine;

enum InteractType {
    SHOP = 0,
    ANIMAL,
    PROPS
}

public class InteractionTarget : MonoBehaviour {
    private Camera thisCamera;
    private GameManager gameManager;
    private InteractionController interactUI;
    [SerializeField] private InteractType type;
    [SerializeField, TextArea(3, 10)] private string content;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        interactUI = FindObjectOfType<InteractionController>();
        thisCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            interactUI.ShowSqure(GetTargetPosition());
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

    //TODO: 열기같은 상호작용 버튼 누르면 CLOSE 해줘야함 
    //TODO: 열기 상호작용시 어떤 상호작용 시켜줘야 함

    public void OpenInteraction() {
        if (type == InteractType.SHOP) {
            OpenShop();
        }
        else {
            interactUI.ShowBubble(content);
        }
    }

    private void OpenShop() {
        gameManager.ChangeState(GameState.Shop);
    }
}
