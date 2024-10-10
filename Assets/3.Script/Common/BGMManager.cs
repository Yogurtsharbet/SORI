using UnityEngine;

public class BGMManager : MonoBehaviour {
    [SerializeField] AudioClip[] bgmClips;
    private AudioSource bgmSource;

    void Start() {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.Play();
    }

    public void StopBGM() {
        bgmSource.Stop();
    }

    public void PlayBGM() {
        bgmSource.Play();
    }

    public void UpdateBGM(int index) {
        bgmSource.clip = bgmClips[index];
    }
}
