using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXController : MonoBehaviour
{
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private void setVolume(float volume) {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
