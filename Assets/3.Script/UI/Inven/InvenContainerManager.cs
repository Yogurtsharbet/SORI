using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenContainerManager : MonoBehaviour {
    private InvenMainController invenMainContainer;
    private SynthesisController synthesisContainer;
    private TabsManager tabsManager;

    private void Awake() {
        invenMainContainer = FindObjectOfType<InvenMainController>();
        synthesisContainer = FindObjectOfType<SynthesisController>();
        tabsManager = FindObjectOfType<TabsManager>();
    }

    private void Start() {
        ClickMainInvenTab();
    }

    public void ClickMainInvenTab() {
        invenMainContainer.OpenInvenMainContainer();
        synthesisContainer.CloseSyntheContianer();
        tabsManager.MainTabActive();
    }

    public void ClickSynthesisTab() {
        invenMainContainer.CloseInvenMainContainer();
        synthesisContainer.OpenSyntheContainer();
        tabsManager.SynthesisTabActive();
    }

    public void ClickInvenContainerClose() {
        gameObject.SetActive(false);
    }

    public void InvenContianerOpen() {
        gameObject.SetActive(true);
    }
}
