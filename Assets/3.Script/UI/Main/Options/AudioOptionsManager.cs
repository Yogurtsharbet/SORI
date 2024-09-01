using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOptionsManager : MonoBehaviour {
    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenAudioOption() {
        gameObject.SetActive(true);
    }

    public void CloseAudioOption() {
        gameObject.SetActive(true);
    }
}
