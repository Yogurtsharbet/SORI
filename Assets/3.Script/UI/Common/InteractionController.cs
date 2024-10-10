using UnityEngine;

public enum InteractType {
    Normal = 0,
    SHOP,
    ANIMAL,
    PROPS
}

public class InteractionController : MonoBehaviour {
    private InteractBubble bubble;
    private InteractSquare square;
    private GameManager gameManager;

    private bool isOpenSquare = false;
    private bool isOpenBubble = false;
    public bool IsOpenSquare => isOpenSquare;

    private InteractType type = InteractType.Normal;

    private void Awake() {
        gameManager = FindObjectOfType<GameManager>();
        bubble = GetComponentInChildren<InteractBubble>();
        square = GetComponentInChildren<InteractSquare>();
    }

    private void Start() {
        bubble.gameObject.SetActive(false);
        square.gameObject.SetActive(false);
    }

    public void ShowBubble(string content) {
        bubble.OpenBubble(content);
        isOpenBubble = true;
    }

    public void HideBubble() {
        bubble.CloseBubble();
        isOpenBubble = false;
    }

    public void ShowSqure(Vector3 pos, InteractType _type) {
        square.OpenSquare(pos);
        type = _type;
        isOpenSquare = true;
    }

    public void HideSqure() {
        square.CloseSquare();
        isOpenSquare = false;
    }

    public void OpenInteraction() {
        if (isOpenSquare || isOpenSquare) {
            if (type == InteractType.SHOP) {
                OpenShop();
                HideSqure();
            }
            else {
                ShowBubble("");
            }
        }
    }

    private void OpenShop() {
        InteractionTarget[] interactionTargets = FindObjectsOfType<InteractionTarget>();
        for (int i = 0; i < interactionTargets.Length; i++)
            interactionTargets[i].isTargetBound = false;
        gameManager.ChangeState(GameState.Shop);
    }

    public void ResetType() {
        type = InteractType.Normal;
    }

    public void UpdateSquarePosition(Vector3 pos) {
        square.UpdatePosition(pos);
    }
}
