using UnityEngine;

public class PlayerSFXController : MonoBehaviour {
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayJump() {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.Play();
    }
}
