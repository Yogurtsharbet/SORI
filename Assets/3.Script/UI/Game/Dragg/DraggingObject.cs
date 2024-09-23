using UnityEngine;

public class DraggingObject : MonoBehaviour{
    protected Canvas canvas;
    protected RectTransform originalParent;
    protected Vector2 originalPosition;
    protected RectTransform rectTransform;            //해당 gameobject transform

    private void Awake() {
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas cn in canvases) {
            if (cn.name.Equals("GameCanvas")) {
                canvas = cn;
            }
        }
        rectTransform = gameObject.GetComponent<RectTransform>();
    }
}
