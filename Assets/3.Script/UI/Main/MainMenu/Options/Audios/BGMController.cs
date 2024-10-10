using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class BGMController : MonoBehaviour {
    private Slider slider;
    public AudioMixer audioMixer;
    private OptionDataManager optionDataManager;

    private void Awake() {
        optionDataManager = FindObjectOfType<OptionDataManager>();
        slider = GetComponentInChildren<Slider>();
    }

    private void OnEnable() {
        slider.value = optionDataManager.OptionData.BgmAudioValue;
        checkVolume(optionDataManager.OptionData.BgmAudioValue);
    }

    private void Start() {
        float volume = PlayerPrefs.GetFloat("BGMValue", 1.0f);
        optionDataManager.OptionData.SetBgmAudioValue(volume);

        slider.onValueChanged.AddListener(delegate {
            setVolume(slider.value);
        });
    }

    private void checkVolume(float volume) {
        if (volume <= 0) {
            audioMixer.SetFloat("BGM", -80);
        }
        else {
            audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        }
    }

    private void setVolume(float volume) {
        if (volume <= 0) {
            optionDataManager.OptionData.SetBgmAudioValue(0);
            PlayerPrefs.SetFloat("BGMValue", 0);
            PlayerPrefs.Save();
        }
        else {
            optionDataManager.OptionData.SetBgmAudioValue(volume);
            PlayerPrefs.SetFloat("BGMValue", volume);
            PlayerPrefs.Save();
        }
        checkVolume(volume);
    }
}