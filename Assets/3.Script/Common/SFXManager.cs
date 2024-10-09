using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

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
            audio.playOnAwake = false;
            sources.Add(audio);
        }
    }

    private void OnEnable() {
        if (envSfx) {
            PlayEnv();
            PlayWaterSfx();
        }
    }

    private void PlayEnv() {
        sources[0].volume = 0.3f;
        sources[0].loop = true;
        sources[0].Play();
    }

    private void PlayEnv(int num) {
        sources[num].Play();
    }

    public void PlayBrokenRockSfx() {
        sources[2].time = 0.5f;
        sources[2].Play();
    }

    private void PlayWaterSfx() {
        sources[1].loop = true;
        sources[1].volume = 0;
        sources[1].Play();
        StopCoroutine(WaterSfx());
        StartCoroutine(WaterSfx());
    }

    private IEnumerator WaterSfx() {
        Vector3[] waterPositions = {
            new Vector3 (479, 0, 386),
            new Vector3 (418, 0, 439),
            new Vector3 (362, 0, 483),
            new Vector3 (481, 0, 324),
            new Vector3 (504, 0, 285)
        };

        float minDistance;
        while (true) {
            yield return null;

            minDistance = Mathf.Infinity;
            foreach (var pos in waterPositions) {
                float distance = Vector3.Distance(transform.parent.position, pos);
                if (distance < minDistance) 
                    minDistance = distance;
            }

            float volume = Mathf.Clamp01((100f - minDistance) / 80f);
            sources[1].volume = volume;
        }
    }
}
