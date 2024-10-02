using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {
    private Image icon;
    private Text questText;

    private void Awake() {
        icon = GetComponentInChildren<Image>();
        questText = GetComponentInChildren<Text>();
    }

    private void Start() {
        ResetQuestText();
    }

    public  void SetQuestText(string contents) {
        icon.enabled = true;
        questText.text = contents;
    }

    public void ResetQuestText() {
        icon.enabled = false;
        questText.text = string.Empty;
    }
}
