using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXController : MonoBehaviour {
    private OptionDataManager optionDataManager;
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        optionDataManager = FindObjectOfType<OptionDataManager>();
        slider = GetComponentInChildren<Slider>();
    }

    private void Start() {
        slider.value = optionDataManager.OptionData.SfxAudionValue;
        checkVolume(optionDataManager.OptionData.SfxAudionValue);

        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private void checkVolume(float volume) {
        if (volume <= 0) {
            audioMixer.SetFloat("SFX", -80);
        }
        else {
            audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        }
    }

    private void setVolume(float volume) {
        if (volume <= 0) {
            optionDataManager.OptionData.SetSfxAudionValue(0);
        }
        else {
            optionDataManager.OptionData.SetSfxAudionValue(volume);
        }
        checkVolume(volume);
    }
}
