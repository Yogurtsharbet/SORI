using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlOptionsManager : MonoBehaviour {
    private MainDetailManager mainDetailManager;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenControlOption() {
        mainDetailManager.CloseDetails();
        gameObject.SetActive(true);
    }

    public void CloseControlOption() {
        gameObject.SetActive(false);
    }
}
