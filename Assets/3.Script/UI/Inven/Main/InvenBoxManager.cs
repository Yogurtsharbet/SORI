using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenBoxManager : MonoBehaviour {
    GameObject[] boxObject = new GameObject[21];
    PlayerInvenController playerInvenController;

    private void Awake() {
        playerInvenController = FindObjectOfType<PlayerInvenController>();
        Button[] boxButtons = GetComponentsInChildren<Button>();
        for (int i = 0; i < boxObject.Length; i++) {
            boxObject[i] = boxButtons[i].gameObject;
            boxObject[i].SetActive(false);
            boxObject[i].GetComponent<InvenBoxController>().SetKey(i);
        }
    }

    private void Start() {
        playerInvenController.InvenChanged += updateBox;
    }

    private void OnDestroy() {
        playerInvenController.InvenChanged -= updateBox;
    }

    private void updateBox(List<Word> inventory) {
        for(int i = 0; i < 21; i++) {
            boxObject[i].SetActive(false);
        }

        for( int i = 0; i < inventory.Count; i++) {
            boxObject[i].SetActive(true);
            boxObject[i].GetComponent<InvenBoxController>().SetWordData(inventory[i]);
        }
    }

}
