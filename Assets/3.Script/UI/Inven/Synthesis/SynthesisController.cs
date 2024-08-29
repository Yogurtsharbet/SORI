using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynthesisController : MonoBehaviour {
    public void OpenSyntheContainer() {
        gameObject.SetActive(true);
    }

    public void CloseSyntheContianer() {
        gameObject.SetActive(false);
    }
}
