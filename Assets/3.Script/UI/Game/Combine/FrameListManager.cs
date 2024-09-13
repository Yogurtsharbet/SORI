using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameListManager : MonoBehaviour {
    [SerializeField] private GameObject slotPrefab;

    private List<Frame> frameList = new List<Frame>();

    private Slider slider;
    private int poolingCount = 10;
    private List<GameObject> slotList = new List<GameObject>();
    private List<RectTransform> slotListRectTransform = new List<RectTransform>();
    private FrameSlotController[] frameSlotConts;

    private Vector3[] initialPositions = new Vector3[10];

    private float previousValue = 0; // 이전 스크롤바 값

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
        slider.minValue = 0;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        frameSlotConts = new FrameSlotController[poolingCount];

        for (int i = 0; i < poolingCount; i++) {
            initialPositions[i] = new Vector3(-20f, 305f - 145f * i, 0f);
            GameObject newSlotObject = Instantiate(slotPrefab, gameObject.transform);
            newSlotObject.transform.localPosition = initialPositions[i];
            newSlotObject.name = $"SentenceSlot{i}";
            newSlotObject.SetActive(false);
            slotListRectTransform.Add(newSlotObject.GetComponent<RectTransform>());
            slotList.Add(newSlotObject);
            frameSlotConts[i] = newSlotObject.GetComponent<FrameSlotController>();
        }
    }

    private void Start() {
        for (int i = 0; i < 20; i++) {
            frameList.Add(new Frame());

            if (i < poolingCount) {
                frameSlotConts[i].SetFrameData(frameList[i]);
            }
        }

        CheckScrollbar();
        slider.maxValue = frameList.Count;

        for (int i = 0; i < slotList.Count; i++) {
            if (slotList[i].transform.localPosition.y > 450f) {
                slotList[i].SetActive(false);
            }
            else if (slotList[i].transform.localPosition.y < -420f) {
                slotList[i].SetActive(false);
            }
            else {
                slotList[i].SetActive(true);
            }
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

    public void OnSliderValueChanged(float value) {
        for (int i = 0; i < poolingCount; i++) {
            initialPositions[i].y += (value - previousValue) * 145f;
            if (initialPositions[i].y > 605f) initialPositions[i].y -= 1450f;
            if (initialPositions[i].y < -845f) initialPositions[i].y += 1450f;
            slotList[i].transform.DOLocalMoveY(initialPositions[i].y, 0.02f).SetEase(Ease.Linear);

            if (slotList[i].transform.localPosition.y > 480f) {
                slotList[i].SetActive(false);
            }
            else if (slotList[i].transform.localPosition.y < -440f) {
                slotList[i].SetActive(false);
            }
            else {
                slotList[i].SetActive(true);
            }
        }
        previousValue = value;
    }
}
