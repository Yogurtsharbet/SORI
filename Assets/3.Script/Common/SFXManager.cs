using UnityEngine;

public class SFXManager : MonoBehaviour {
    [SerializeField] AudioClip[] audioClips;
    private AudioSource sfxSource;

    private void Awake() {
        sfxSource = GetComponent<AudioSource>();
    }

    public void PlayJump() {
        int randomIndex = Random.Range(0, audioClips.Length);
        sfxSource.clip = audioClips[randomIndex];
        sfxSource.Play();
    }
}
