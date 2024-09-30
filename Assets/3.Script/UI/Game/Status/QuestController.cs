using UnityEngine;
using UnityEngine.UI;

public class QuestController : MonoBehaviour {
    private Text questText;

    private void Awake() {
        questText = GetComponentInChildren<Text>();
    }

    private void Start() {
        ResetQuestText();
    }

    public  void SetQuestText(string contents) {
        questText.text = contents;
    }

    public void ResetQuestText() {
        questText.text = string.Empty;
    }
}
