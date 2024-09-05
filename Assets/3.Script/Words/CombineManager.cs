using UnityEngine;
using Cinemachine;

public class CombineManager : MonoBehaviour {
    private CinemachineFreeLook cameraSelectView;
    private Frame frame;

    private void Start() {
        cameraSelectView = CameraControl.Instance.GetComponentInChildren<CinemachineFreeLook>();
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
        // 타겟 찾는 도중 ESC로 취소하기
        // 타겟을 찾은 뒤에는 성공 이펙트를 출력하고 효과 적용하기
        CameraControl.Instance.SetCamera(cameraSelectView);

        // 오브젝트를 찾는 로직 (외곽선)
        // 찾은 오브젝트 -> Activate (object)
    }
}

