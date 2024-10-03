using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MasterController : MonoBehaviour {
    private OptionDataManager optionDataManager;
    private Slider slider;
    public AudioMixer audioMixer;

    private void Awake() {
        optionDataManager = FindObjectOfType<OptionDataManager>();
        slider = GetComponentInChildren<Slider>();
    }
    private void OnEnable() {
        slider.value = optionDataManager.OptionData.MasterAudioValue;
        checkVolume(optionDataManager.OptionData.MasterAudioValue);        
    }

    private void Start() {
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
            optionDataManager.OptionData.SetMasetAudionValue(0);
        }
        else {
            optionDataManager.OptionData.SetMasetAudionValue(volume);
        }
        checkVolume(volume);
    }
}
