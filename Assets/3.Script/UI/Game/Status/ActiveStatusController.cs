using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// [UI] 활성 정보 - 활성화 중인 프레임 정보 창
// 마우스 오버시 프레임목록 오픈, 현재 활성화중인 프레임 표시
public class ActiveStatusController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Text activeCountTxt;
    private int activeCount = 0;
    private FrameListContainer sentenceManager;
    public static bool IsOpenFrameList = false;

    private Vector3 openPosition = new Vector3(-750f, 0, 0);
    private Vector3 closedPosition = new Vector3(-1200f, 0, 0);

    private bool isHoveringButton = false;  // 버튼 위에 마우스가 있는지
    private bool isHoveringTarget = false;  // 패널 위에 마우스가 있는지
    private Coroutine moveCoroutine = null; // 현재 실행 중인 이동 코루틴

    private float moveSpeed = 5f;    // 이동 속도

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        sentenceManager = FindObjectOfType<FrameListContainer>();
        foreach (Text txt in texts) {
            if (txt.name.Equals("Count"))
                activeCountTxt = txt;
        }
    }

    public void ActiveCountUpdate(int num) {
        activeCount = num;
        activeCountTxt.text = "x" + num;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHoveringButton = true;
        OpenTarget();
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHoveringButton = false;
        StartCoroutine(CheckForReturn());
        IsOpenFrameList = false;
    }

    public void OnTargetPointerEnter() {
        isHoveringTarget = true;
        OpenTarget();
    }

    public void OnTargetPointerExit() {
        isHoveringTarget = false;
        StartCoroutine(CheckForReturn());
        IsOpenFrameList = false;
    }

    private void OpenTarget() {
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveObject(openPosition));
        IsOpenFrameList = true;
    }

    private IEnumerator CheckForReturn() {
        yield return new WaitForSeconds(0.1f);
        if (!isHoveringButton && !isHoveringTarget) {
            if (moveCoroutine != null) {
                StopCoroutine(moveCoroutine);
            }
            moveCoroutine = StartCoroutine(MoveObject(closedPosition));
        }
    }

    private IEnumerator MoveObject(Vector3 targetPosition) {
        if (sentenceManager == null) sentenceManager = FindObjectOfType<FrameListContainer>();

        while (Vector3.Distance(sentenceManager.gameObject.transform.localPosition, targetPosition) > 0.01f) {
            sentenceManager.gameObject.transform.localPosition = Vector3.Lerp(sentenceManager.gameObject.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        sentenceManager.gameObject.transform.localPosition = targetPosition;
        moveCoroutine = null;
    }
}
