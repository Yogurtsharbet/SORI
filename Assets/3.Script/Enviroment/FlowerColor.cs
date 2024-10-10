using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlowerColorType {
    Red, Blue, Yellow, Green, Orange
}

public class FlowerColor : MonoBehaviour {
    public FlowerColorType color;
    private StarCoinManager starCoinManager;
    private bool starCoinFlag;

    private void Awake() {
        GetComponent<Renderer>().material.color = GetFlowerColor();
        starCoinManager = GameManager.Instance.starCoinManager;
    }

    private Color GetFlowerColor() {
        switch (color) {
            case FlowerColorType.Red:
                return new Color(0.85f, 0.3f, 0.3f);
            case FlowerColorType.Blue:
                return new Color(0.5f, 0.65f, 0.8f);
            case FlowerColorType.Yellow:
                return new Color(0.8f, 0.65f, 0.25f);
            case FlowerColorType.Green:
                return new Color(0.4f, 0.75f, 0.35f);
            case FlowerColorType.Orange:
                return new Color(0.85f, 0.5f, 0.3f);
            default:
                return new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !starCoinFlag) {
            starCoinFlag = true;
            starCoinManager.SpawnCoin(transform.position);
        }
    }
}
