using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenContainerManager : MonoBehaviour {

    private void Awake() {
    }

    public void OpenInventory() {
        gameObject.SetActive(true);
    }

    public void CloseInventory() {
        //TODO: temp inventory�� inventory �ٲٱ�
        //TODO: �ռ�â �ȿ� �ܾ� ���� �ϱ�
        gameObject.SetActive(false);
    }
}
