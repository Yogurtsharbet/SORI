using UnityEngine;

[CreateAssetMenu(fileName = "OptionData", menuName = "ScriptableObjects/OptionData", order = 1)]
public class OptionData : ScriptableObject {

    [Header("볼륨")]
    [SerializeField, Range(0f, 1f)]
    private float masterAudionValue = 1f;
    public float MasterAudioValue => masterAudionValue;
    public void SetMasetAudionValue(float value) {
        masterAudionValue = value;
    }

    [SerializeField, Range(0f, 1f)] private float bgmAudionValue = 1f;
    public float BgmAudioValue => bgmAudionValue;
    public void SetBgmAudioValue(float value) {
        bgmAudionValue = value;
    }

    [SerializeField, Range(0f, 1f)] private float sfxAudionValue = 1f;
    public float SfxAudionValue => sfxAudionValue;
    public void SetSfxAudionValue(float value) {
        sfxAudionValue = value;
    }

    [Space]
    [Header("UI")]
    [SerializeField] private bool isFullScreen = true;
    public bool IsFullScreen => isFullScreen;
    public void SetIsFullScreen(bool yn) {
        isFullScreen = yn;
    }

    [SerializeField] private ResolutionType resolutionType = ResolutionType.FullHD;
    public ResolutionType ResolutionType => resolutionType;
    public void SetResolutionType(ResolutionType type) {
        resolutionType = type;
    }

    [SerializeField, Range(0f, 1f)] private float uIScale = 1f;
    public float UIScale => UIScale;
    public void SetUIScale(float value) {
        uIScale = value;
    }

    [SerializeField] private bool activeVSync = true;
    public bool ActiveVSync => activeVSync;
    public void SetActiveVSync(bool yn) {
        activeVSync = yn;
    }
}
