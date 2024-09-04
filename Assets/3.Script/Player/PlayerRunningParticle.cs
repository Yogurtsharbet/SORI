using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningParticle : MonoBehaviour {
    private ParticleSystem runningParticle;
    private ParticleSystem jumpingParticle;

    private ParticleSystem.EmissionModule runningParticleEmission;
    private void Awake() {
        runningParticle = GetComponentsInChildren<ParticleSystem>()[0];
        jumpingParticle = GetComponentsInChildren<ParticleSystem>()[1];

        runningParticleEmission = runningParticle.emission;
    }

    public void SetDustRate(float speed) {
        runningParticleEmission.rateOverTime = speed * 0.285714f;
    }

    public void Jump() {
        jumpingParticle.Play();
    }

    public void Move() {
        runningParticle.Play();
    }

    public void Stop() {
        runningParticle.Stop();
    }
}