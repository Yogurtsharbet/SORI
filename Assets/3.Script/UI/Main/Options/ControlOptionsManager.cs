using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlOptionsManager : MonoBehaviour {
    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenControlOption() {
        gameObject.SetActive(true);
    }

    public void CloseControlOption() {
        gameObject.SetActive(true);
    }
}
