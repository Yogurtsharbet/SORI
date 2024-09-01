using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicOptionsManager : MonoBehaviour {
    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenGraphicOption() {
        gameObject.SetActive(true);
    }

    public void CloseGraphicOption() {
        gameObject.SetActive(true);
    }
}
