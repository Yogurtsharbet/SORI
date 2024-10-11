using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugControl : MonoBehaviour
{
    private Text text;
    private void Awake() {
        text = GetComponent<Text>();
    }

    public void SetText(string t) {
        text.text += "\n" + t;
    }
}
