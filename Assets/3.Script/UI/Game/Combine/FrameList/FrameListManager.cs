using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [UI] 문장 목록 - 문장 목록 관리 매니저
public class FrameListManager : MonoBehaviour {
    [SerializeField] private GameObject slotPrefab;
    private int poolingCount = 10;
    private Slider slider;
    private List<GameObject> slotList = new List<GameObject>();
    private FrameListSlotController[] frameListSlotControllers;

    private List<Frame> frameList = new List<Frame>();

    private Vector3[] initialPositions = new Vector3[10];

    private float previousValue = 0; // 이전 스크롤바 값

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
        slider.minValue = 0;
        slider.onValueChanged.AddListener(OnSliding);
        frameListSlotControllers = new FrameListSlotController[poolingCount];

        for (int i = 0; i < poolingCount; i++) {
            initialPositions[i] = new Vector3(-20f, 305f - 145f * i, 0f);
            GameObject newSlotObject = Instantiate(slotPrefab, gameObject.transform);
            newSlotObject.transform.localPosition = initialPositions[i];
            newSlotObject.name = $"SentenceSlot{i}";
            newSlotObject.SetActive(false);
            slotList.Add(newSlotObject);
            frameListSlotControllers[i] = newSlotObject.GetComponent<FrameListSlotController>();
        }
    }

    private void Start() {
        testData(); //TODO: 테스트 데이터 지우기

        CheckScrollbar();
        slider.maxValue = frameList.Count;

        if (frameList.Count >= 6) {
            for (int i = 0; i < poolingCount; i++) {
                if(i < frameList.Count) { 
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

    private void testData() {
        for (int i = 0; i < 8; i++) {
            frameList.Add(new Frame());
        }
    }

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
            if (initialPositions[i].y < -845f) {
                initialPositions[i].y += 1450f;
                UpdateSlotData(i);
            }
            slotList[i].transform.DOLocalMoveY(initialPositions[i].y, 0.02f).SetEase(Ease.Linear);

            if (slotList[i].transform.localPosition.y > 480f || slotList[i].transform.localPosition.y < -440f) {
                slotList[i].SetActive(false);
            }
            else {
                slotList[i].SetActive(true);
            }
        }
        previousValue = value;
    }

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
        if(frameList.Count < 6) {
            frameListMinSetting();
        }
        else {
            int value = (int)scrollValue;
            for (int i = 0; i < poolingCount; i++) {
                frameListSlotControllers[i].SetFrameData(frameList[i + value]);
                frameListSlotControllers[i].SetKey(i + value);
            }
        }
    }

    public void SortingToActive() {
        frameList.Sort((x, y) => y.IsActive.CompareTo(x.IsActive));
        UpdateSlotData(previousValue);
    }

    public void SortingIsEmpty() {
        frameList.Sort((x, y) => x.IsActive.CompareTo(y.IsActive));
        UpdateSlotData(previousValue);
    }

    public void DeleteFrame(int index) {
        frameList.RemoveAt(index);
        UpdateSlotData(previousValue);
    }

    public void AddFrame(Frame frame = null) {
        if (frame != null) {
            frameList.Add(frame);
        }
        else {
            frameList.Add(new Frame());
        }
        UpdateSlotData(previousValue);
    }
}
