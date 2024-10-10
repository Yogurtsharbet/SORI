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
        if (GameManager.Instance.currentScene != "Map")
            ResetQuestText();
        else if (GameManager.Instance.isCompleteTutorial)
            SetQuestText("모험을 마쳤습니다. 집으로 다시 돌아갈까요?");
    }

    public void SetQuestText(string contents) {
        icon.enabled = true;
        questText.text = contents;
    }

    public void ResetQuestText() {
        icon.enabled = false;
        questText.text = string.Empty;
    }
}
