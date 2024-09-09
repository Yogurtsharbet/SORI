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
        // UI를 끄고 카메라를 전환해서 타겟 찾기   
        // ( DONE ) 타겟 찾는 도중 ESC로 취소하기 
        // 타겟을 찾은 뒤에는 성공 이펙트를 출력하고 효과 적용하기

        Debug.Log(frame.wordA.Name);
        selectControl.SetTargetTag(frame.wordA.ToTag());
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);

        // 오브젝트를 찾는 로직 (외곽선)
        // 찾은 오브젝트 -> Activate (object)
        // frame.Activate();
    }
}
