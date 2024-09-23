using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// [UI] 문장 목록 - 문장 목록 관리 매니저
public class FrameListManager : MonoBehaviour {
    [SerializeField] private GameObject slotPrefab;
    private int poolingCount = 10;
    private FrameListSlotController[] frameListSlotControllers;     //슬롯 controller
    private List<GameObject> slotList = new List<GameObject>();     //슬롯 list
    private List<Frame> frameList = new List<Frame>();              //실제 프레임 데이터 목록
    private Vector3[] initialPositions = new Vector3[10];           //기본 위치값

    private Slider slider;
    private float previousValue = 0; // 이전 스크롤바 값

    private DefaultInputActions inputAction;                        //휠 InputSys
    private ActiveStatusController activeStatus;                    //왼쪽 상단 active status

    private void Awake() {
        inputAction = new DefaultInputActions();
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(OnSliding);
        frameListSlotControllers = new FrameListSlotController[poolingCount];
        slider.minValue = 0;

        for (int i = 0; i < poolingCount; i++) {
            initialPositions[i] = new Vector3(-20f, 305f - 145f * i, 0f);
            GameObject newSlotObject = Instantiate(slotPrefab, gameObject.transform);
            newSlotObject.transform.localPosition = initialPositions[i];
            newSlotObject.name = $"SentenceSlot{i}";
            newSlotObject.SetActive(false);
            slotList.Add(newSlotObject);
            frameListSlotControllers[i] = newSlotObject.GetComponent<FrameListSlotController>();
        }

        inputAction.UI.ScrollWheel.performed += value => OnScroll(value.ReadValue<Vector2>());
        activeStatus =FindObjectOfType<ActiveStatusController>();
    }

    private void Start() {
        testData(); //TODO: 테스트 데이터 지우기

        CheckScrollbar();
        slider.maxValue = frameList.Count;

        if (frameList.Count >= 6) {
            for (int i = 0; i < poolingCount; i++) {
                if (i < frameList.Count) {
                    frameListSlotControllers[i].SetFrameData(frameList[i]);
                    frameListSlotControllers[i].SetKey(i);
                }
            }
            for (int i = 0; i < slotList.Count; i++) {
                if (slotList[i].transform.localPosition.y > 450f || slotList[i].transform.localPosition.y < -420f) {
                    slotList[i].SetActive(false);
                }
                else {
                    slotList[i].SetActive(true);
                }
            }
        }
        else {
            frameListMinSetting();
        }
    }

    private void OnEnable() {
        inputAction.Enable();
    }

    private void OnDisable() {
        inputAction.UI.ScrollWheel.performed -= value => OnScroll(value.ReadValue<Vector2>());
    }

    private void testData() {
        for (int i = 0; i < 8; i++) {
            AddFrame(new Frame());
        }
    }

    #region 스크롤

    private void CheckScrollbar() {
        if (frameList.Count < 6) {
            slider.interactable = false;
        }
        else {
            slider.interactable = true;
        }
    }
   
    public void OnSliding(float value) {
        for (int i = 0; i < poolingCount; i++) {
            initialPositions[i].y += (value - previousValue) * 145f;
            if (initialPositions[i].y > 605f) {
                initialPositions[i].y -= 1450f;
                UpdateSlotData(i);
            }
            else if (initialPositions[i].y < -845f) {
                initialPositions[i].y += 1450f;
                UpdateSlotData(i);
            }
            slotList[i].transform.DOLocalMoveY(initialPositions[i].y, 0.05f).SetEase(Ease.InOutQuad);
            if (slotList[i].transform.localPosition.y > 480f || slotList[i].transform.localPosition.y < -440f) {
                slotList[i].SetActive(false);
            }
            else {
                slotList[i].SetActive(true);
            }
        }
        previousValue = value;
    }

    private void OnScroll(Vector2 value) {
        if (ActiveStatusController.IsOpenFrameList) {
            float scrollDelta = -value.y;
            if (scrollDelta > 0f) {
                slider.value = Mathf.Clamp(slider.value + 0.5f, slider.minValue, slider.maxValue);
            }
            else if (scrollDelta < 0f) {
                slider.value = Mathf.Clamp(slider.value - 0.5f, slider.minValue, slider.maxValue);
            }
        }
    }

    #endregion

    #region 프레임 슬롯 세팅
    //프레임 전체 개수가 6 미만일때 세팅
    private void frameListMinSetting() {
        for (int i = 0; i < frameList.Count; i++) {
            frameListSlotControllers[i].SetFrameData(frameList[i]);
            frameListSlotControllers[i].SetKey(i);
        }
        for (int i = 0; i < slotList.Count; i++) {
            if (i < frameList.Count) {
                slotList[i].SetActive(true);
            }
            else {
                slotList[i].SetActive(false);
            }
        }
    }

    private void UpdateSlotData(float scrollValue) {
        int value = (int)scrollValue;
        for (int i = 0; i < poolingCount; i++) {
            if (i + value < frameList.Count) {
                frameListSlotControllers[i].SetFrameData(frameList[i + value]);
                frameListSlotControllers[i].SetKey(i + value);
            }
            else {
                slotList[i].SetActive(false); // 데이터를 넘어가는 슬롯 비활성화
            }
        }
    }
    #endregion

    #region 프레임 정렬
    public void SortingToActive() {
        frameList.Sort((x, y) => y.IsActive.CompareTo(x.IsActive));
        UpdateSlotData(previousValue);
    }

    public void SortingIsEmpty() {
        frameList.Sort((x, y) => x.IsActive.CompareTo(y.IsActive));
        UpdateSlotData(previousValue);
    }
    #endregion

    public void DeleteFrame(int index) {
        frameList.RemoveAt(index);
        UpdateSlotData(previousValue);
        activeStatus.ActiveCountUpdate(checkActiveFrame());
    }

    public void AddFrame(Frame frame = null) {
        if (frame != null) {
            frameList.Add(frame);
        }
        else {
            frameList.Add(new Frame());
        }
        UpdateSlotData(previousValue);
        activeStatus.ActiveCountUpdate(checkActiveFrame());
    }

    //활성화 개수 체크
    private int checkActiveFrame() {
        int activeCount = 0;
        for (int i = 0; i < frameList.Count; i++) {
            if (frameList[i].IsActive) {
                activeCount++;
            }
        }
        return activeCount;
    }
}
