using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordInfoController : MonoBehaviour {
    private bool isViewInfo = false;

    private Text infoTitle;
    private Text infoContent;

    private PlayerInvenController playerInvenController;

    private void Awake() {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach(Text txt in texts) {
            if (txt.name.Equals("Content")) {
                infoContent = txt;
            }
            else {
                infoTitle = txt;
            }
        }
        playerInvenController = FindObjectOfType<PlayerInvenController>();
    }

    public void WordButtonClick(int index) {
        isViewInfo = !isViewInfo;
        if (isViewInfo) {
            Word selectWord = playerInvenController.GetWordIndex(index);
            infoTitle.text = selectWord.Name;
            infoContent.text = $"¡æ {selectWord.GetRankText()} µî±Þ\n¡æ {selectWord.GetTypeText()}";
        }
        else {
            infoTitle.text = string.Empty;
            infoContent.text = string.Empty;
        }
    }
}
