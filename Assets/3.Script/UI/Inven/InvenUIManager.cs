using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenUIManager : MonoBehaviour {
    private List<GameObject> invenUi = new List<GameObject>();
    [SerializeField] private GameObject invenBoxPrf;

    private PlayerInven inven;
    private int totalInvenCount;

    private void Awake() {
        inven = FindObjectOfType<PlayerInven>();
    }

    private void Start() {
        initInven();    
    }

    private void initInven() {
        totalInvenCount = 18;
        for (int i = 0; i < totalInvenCount; i++) {
            GameObject invenBox = Instantiate(invenBoxPrf);
            invenUi.Add(invenBox);
        }
    }


}
