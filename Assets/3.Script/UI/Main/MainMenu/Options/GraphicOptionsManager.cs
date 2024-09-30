using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicOptionsManager : MonoBehaviour {
    private MainDetailManager mainDetailManager;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();    
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenGraphicOption() {
        mainDetailManager.CloseDetails();
        gameObject.SetActive(true);
    }

    public void CloseGraphicOption() {
        gameObject.SetActive(false);
    }
}
