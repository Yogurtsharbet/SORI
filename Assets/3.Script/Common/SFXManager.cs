using UnityEngine;

public class SFXManager : MonoBehaviour {
    [SerializeField] AudioClip[] audioClips;
    private AudioSource sfxSource;
    [SerializeField] bool envSfx = false;

    private void Awake() {
        sfxSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        if (envSfx) {
            PlayENv();
        }
    }

    public void PlayJump() {
        int randomIndex = Random.Range(0, audioClips.Length);
        sfxSource.clip = audioClips[randomIndex];
        sfxSource.Play();
    }

    private void PlayENv() {
        sfxSource.Play();
    }
}
