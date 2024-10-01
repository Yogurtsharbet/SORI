using UnityEngine;

public class AudioOptionsManager : MonoBehaviour {
    private MainDetailManager mainDetailManager;

    private void Awake() {
        mainDetailManager = FindObjectOfType<MainDetailManager>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenAudioOption() {
        mainDetailManager.CloseDetails();
        gameObject.SetActive(true);
    }

    public void CloseAudioOption() {
        gameObject.SetActive(false);
    }
}
