using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BGMController : MonoBehaviour {
    private MainDetailManager mainDetailManager;
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        slider.value = mainDetailManager.OptionData.BgmAudioValue;
        checkVolume(mainDetailManager.OptionData.BgmAudioValue);

        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private  void checkVolume(float volume) {
        if (volume <= 0) {
            audioMixer.SetFloat("BGM", -80);
        }
        else {
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
    }

    private void setVolume(float volume) {
        if (volume <= 0) {
            mainDetailManager.OptionData.SetBgmAudioValue(0);
        }
        else {
            mainDetailManager.OptionData.SetBgmAudioValue(volume);
        }
        checkVolume(volume);
    }
}