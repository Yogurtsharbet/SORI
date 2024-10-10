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

    private void OnEnable() {
        slider.value = optionDataManager.OptionData.SfxAudionValue;
        checkVolume(optionDataManager.OptionData.SfxAudionValue);
    }

    private void Start() {
        float volume = PlayerPrefs.GetFloat("SFXValue", 1.0f);
        optionDataManager.OptionData.SetSfxAudionValue(volume);

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
            PlayerPrefs.SetFloat("SFXValue", 0);
            PlayerPrefs.Save();
        }
        else {
            optionDataManager.OptionData.SetSfxAudionValue(volume);
            PlayerPrefs.SetFloat("SFXValue", volume);
            PlayerPrefs.Save();
        }
        checkVolume(volume);
    }
}
