using UnityEngine;
using UnityEngine.SceneManagement;

public class WordCardSelectContainer : MonoBehaviour {
    private NewWordCardController[] newWordCardControllers;
    private PlayerInvenController playerInvenController;

    private int[] localPosiont = new int[3] { 0, -350, -620 };

    public GameObject LastSelected { get; set; }
    public int LastSelectedIndex { get; set; }
    public float randomRate = 30;

    private void Awake() {
        newWordCardControllers = GetComponentsInChildren<NewWordCardController>();
        playerInvenController = FindObjectOfType<PlayerInvenController>();
    }

    private void Start() {
        for (int i = 0; i < 3; i++) {
            newWordCardControllers[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);

        if(GameManager.Instance.currentScene != "Map") {
            randomRate = 90f;
            GetWordCard(3);
        }
    }

    public void OpenWordCardSelect() {
        gameObject.SetActive(true);
    }

    //랜덤 카드 생성
    public void GetWordCard(int num) {
        gameObject.SetActive(true);
        ActiveCardObject(num);
        for (int i = 0; i < num; i++) {
            Word newWord = Word.GetWord();
            newWordCardControllers[i].SetWordData(newWord);
            newWordCardControllers[i].StartAppear();
        }
    }

    public void GetWordCard(Word[] word) {
        gameObject.SetActive(true);
        ActiveCardObject(word.Length);
        for (int i = 0; i < word.Length; i++) {
            newWordCardControllers[i].SetWordData(word[i]);
            newWordCardControllers[i].StartAppear();
        }
    }

    private void ActiveCardObject(int num) {
        for (int i = 0; i < num; i++) {
            newWordCardControllers[i].gameObject.SetActive(true);
            if (num == 3) {
                Vector3 newPos = new Vector3(localPosiont[2] - (i * localPosiont[2]), -70f, 0f);
                newWordCardControllers[i].SetCardPositionSetting(newPos);
            }
            else if (num == 2) {
                Vector3 newPos = new Vector3(localPosiont[1] - (i * localPosiont[1]) * 2, -70f, 0f);
                newWordCardControllers[i].SetCardPositionSetting(newPos);
            }
            else {
                Vector3 newPos = new Vector3(localPosiont[0], -70f, 0f);
                newWordCardControllers[i].SetCardPositionSetting(newPos);
            }
        }
    }

    private bool mapFlag;
    public void SelectNewWord(Word word) {
        //TODO: 인벤 다 차있을때 예외처리 -> 이미 있는 단어 삭제 혹은 가져갈 수 없음~~
        playerInvenController.AddNewItem(word);
        for (int i = 0; i < 3; i++) {
            newWordCardControllers[i].gameObject.SetActive(false);
        }
        gameObject.SetActive(false);

        if (SceneManager.GetActiveScene().name == "Map" && !mapFlag) {
            mapFlag = true;
            Word[] newWord = new Word[2];
            newWord[0] = Word.GetWord(WordData.Search("MOVE").Key);
            newWord[1] = Word.GetWord(WordData.Search("DISAPPEAR").Key);
            GetWordCard(newWord);
        }
        else {
            if(Random.Range(0f, 100f) < randomRate) {
                randomRate *= 0.12f;
                GetWordCard(Random.Range(1, 4));
            }
        }
    }
}
