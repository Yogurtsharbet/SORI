using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterController : MonoBehaviour {
    private MainDetailManager mainDetailManager;
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        slider.value = mainDetailManager.OptionData.MasterAudioValue;
        checkVolume(mainDetailManager.OptionData.MasterAudioValue);

        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private void checkVolume(float volume) {
        if (volume <= 0) {
            audioMixer.SetFloat("Master", -80);
        }
        else {
            audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        }
    }

    private void setVolume(float volume) {
        if (volume <= 0) {
            mainDetailManager.OptionData.SetMasetAudionValue(0);
        }
        else {
            mainDetailManager.OptionData.SetMasetAudionValue(volume);
        }
        checkVolume(volume);
    }
}
