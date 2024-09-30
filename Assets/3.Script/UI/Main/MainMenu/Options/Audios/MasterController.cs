using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterController : MonoBehaviour
{
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        slider.value = 1f;

        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private void setVolume(float volume) {
        if (volume <= 0) {
            audioMixer.SetFloat("Master", -80); // 최소 볼륨으로 설정 (음소거)
        }
        else {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
    }
}
