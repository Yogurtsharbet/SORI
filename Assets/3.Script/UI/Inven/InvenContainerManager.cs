using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenContainerManager : MonoBehaviour {
    private SynthesisManager synthesisManager;

    private void Awake() {
        synthesisManager = FindObjectOfType<SynthesisManager>();
    }

    public void OpenInventory() {
        gameObject.SetActive(true);
    }

    public void CloseInventory() {
        synthesisManager.ResetAllSlot();
        gameObject.SetActive(false);
    }
}
