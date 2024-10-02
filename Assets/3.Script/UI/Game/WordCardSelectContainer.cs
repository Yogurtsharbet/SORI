using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCardSelectContainer : MonoBehaviour {
    [SerializeField] private GameObject wordCardPrefab;
    private List<NewWordCardController> newWordCardControllers = new List<NewWordCardController>();
    private PlayerInvenController playerInvenController;

    private int[] localPosiont = new int[3] { 0, -350, -620 };

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    public void OpenWordCardSelect() {
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        
    }

    public void NewWordTestData() {
        GetWordCard(3);
    }

    //랜덤 카드 생성
    public void GetWordCard(int num) {
        CreateCardObject(num);
        for (int i = 0; i < num; i++) {
            Word newWord = Word.GetWord();
            newWordCardControllers[i].GetCardComponent();
            newWordCardControllers[i].SetWordData(newWord);
        }
    }

    public void GetWordCard(Word[] word) {
        CreateCardObject(word.Length);
        for(int i = 0; i < newWordCardControllers.Count; i++) {
            newWordCardControllers[i].SetWordData(word[i]);
        }
    }

    private void OnDisable() {
        newWordCardControllers.RemoveRange(0, newWordCardControllers.Count);
    }

    //TODO: INSTANTIATE 하지말고 3개짜리, 2개짜리, 1개짜리 다 만들어서 열고 끄는거로 수정해야함

    private void CreateCardObject(int num) {
        if (num > 2) {
            for (int i = 0; i < 3; i++) {
                GameObject wordCard = Instantiate(wordCardPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                wordCard.transform.localPosition = new Vector3(localPosiont[2] - (i * localPosiont[2]), 0f, 0f);
                wordCard.name = $"WordCard{i}";
                newWordCardControllers.Add(wordCard.GetComponent<NewWordCardController>());
            }
        }
        else if (num > 1) {
            for (int i = 0; i < 2; i++) {
                GameObject wordCard = Instantiate(wordCardPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                if (i == 0) {
                    wordCard.transform.localPosition = new Vector3(localPosiont[1] - (i * localPosiont[1]), 0f, 0f);
                }
                else {
                    wordCard.transform.localPosition = new Vector3(localPosiont[1] - (2 * localPosiont[1]), 0f, 0f);
                }
                wordCard.name = $"WordCard{i}";
                newWordCardControllers.Add(wordCard.GetComponent<NewWordCardController>());
            }
        }
        else {
            GameObject wordCard = Instantiate(wordCardPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            wordCard.transform.localPosition = new Vector3(0f, 0f, 0f);
            wordCard.name = $"WordCard{0}";
                newWordCardControllers.Add(wordCard.GetComponent<NewWordCardController>());
        }
    }

    public void SelectNewWord(Word word) {
        //TODO: 인벤 다 차있을때 예외처리 -> 이미 있는 단어 삭제 혹은 가져갈 수 없음~~
        playerInvenController.AddNewItem(word);
    }
}
