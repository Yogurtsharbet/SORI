using Cinemachine;
using UnityEngine;

public class CombineManager : MonoBehaviour {
    private SelectControl selectControl;
    private Frame frame;

    private void Awake() {
        selectControl = FindObjectOfType<SelectControl>();
    }

    private void Start() {
        test();
    }

    private void test() {
        while (true) {
            Frame newFrame = new Frame(FrameType.AisB, FrameRank.NORMAL);
            OnFrameSet(newFrame);
            Word newWord = Word.GetWord();
            if (OnWordSet(newWord))
                if (CheckValidity())
                    SelectMode();
            newWord = Word.GetWord();
            if (OnWordSet(newWord))
                if (CheckValidity()) {
                    SelectMode();
                    break;
                }
        }
    }

    public void OnFrameSet(Frame frame) {
        this.frame = frame;
    }

    public bool OnWordSet(Word word) {
        // 프레임에 단어를 놓을 수 있을 경우 true 를 return.
        //TODO: UI는 true가 return된 경우에만 단어카드 UI를 프레임 위에 올릴 것
        return frame.SetWord(word);
    }

    public bool CheckValidity() {
        return frame.CheckSentenceValidity();
    }

    public void SelectMode() {
        //TODO: 여기서 문장조합창 UI를 끄기
        Debug.Log("선택가능 오브젝트 : " + frame.wordA.Name);
        selectControl.SetTargetTag(frame.wordA.ToTag());
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
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
