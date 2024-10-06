using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : MonoBehaviour {
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] bool envSfx = false;
    private List<AudioSource> sources = new List<AudioSource>();
    [SerializeField] private AudioMixerGroup audioMixerGroup;

    private void Awake() {
        setAudioSorce();
    }

    private void setAudioSorce() {
        for (int i = 0; i < audioClips.Length; i++) {
            GameObject sourceObj = new GameObject("AudioSource" + i);
            sourceObj.transform.parent = transform;

            AudioSource audio = sourceObj.AddComponent<AudioSource>();
            audio.clip = audioClips[i];
            audio.outputAudioMixerGroup = audioMixerGroup;
            sources.Add(audio);
        }
    }

    private void OnEnable() {
        if (envSfx) {
            PlayEnv();
        }
    }

    private void PlayEnv() {
        sources[0].Play();
    }

    private void PlayEnv(int num) {
        sources[num].Play();
    }
}
