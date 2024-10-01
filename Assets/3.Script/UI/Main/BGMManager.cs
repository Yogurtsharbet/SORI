using UnityEngine;

public class BGMManager : MonoBehaviour {
    private AudioSource bgmSource;

    void Start() {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.Play();
    }

    public void StopBGM() {
        bgmSource.Stop();
    }
}
