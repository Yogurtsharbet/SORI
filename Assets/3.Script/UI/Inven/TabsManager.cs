using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour {
    private Image mainInvenTab;
    private Image synthesisTab;

    private Color activeColor = new Color(0.47f, 0.7f, 0.87f);
    private Color disActiveColor = new Color(0.74f, 0.85f, 0.94f);

    private void Awake() {
        Image[] images = GetComponentsInChildren<Image>();
        foreach(Image img in images) {
            if (img.name.Equals("InvenTab")) {
                mainInvenTab = img;
            }else if (img.name.Equals("SynthesisTab")) {
                synthesisTab = img;
            }
        }
    }

    public void MainTabActive() {
        mainInvenTab.color = activeColor;
        synthesisTab.color = disActiveColor;
    }

    public void SynthesisTabActive() {
        mainInvenTab.color = disActiveColor;
        synthesisTab.color = activeColor;
    }
}
