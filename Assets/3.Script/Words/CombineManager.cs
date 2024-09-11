using Cinemachine;
using UnityEngine;

public class CombineManager : MonoBehaviour {
    private SelectControl selectControl;
    private Frame frame;

    private void Awake() {
        selectControl = FindObjectOfType<SelectControl>();
    }

    public bool CheckValidity() {
        return frame.CheckSentenceValidity();
    }

    public void Activate(GameObject target, GameObject indicator) {
        frame.Activate(target, indicator);
    }

    //FOR DEBUGGING
    public WordKey key;
    private void OnValidate() {
        if (frame != null) {
            frame.SetWordA_DEBUGGING(key);
            selectControl.SetTargetTag(frame.wordA.ToTag());
            Debug.Log("선택가능 오브젝트 : " + frame.wordA.Name);
        }
    }
}
