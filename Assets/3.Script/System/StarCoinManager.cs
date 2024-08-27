using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �׾Ƹ�, ���� ��� ������ ������ �ϱ�.
// ���� :
//      FindObjectOfType<CoinDropControl>().SpawnCoin(transform.position);

public class StarCoinManager : MonoBehaviour {
    [SerializeField] private GameObject starCoinPrefab;
    [SerializeField] private Text starCoinUI;
    private List<GameObject> starCoinPool;
    
    private int minSpawnCount = 3;
    private int maxSpawnCount = 10;

    public bool debugTrigger;

    private void Awake() {
        starCoinPool = new List<GameObject>();
    }

    private void Update() {
        if (debugTrigger) {
            debugTrigger = false;
            SpawnCoin(transform.position);
        }
    }

    private void SpawnCoin(Vector3 spawnPosition) {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount);
        Vector3 explosionPosition = spawnPosition;
        explosionPosition.y -= 1f;

        foreach(var eachCoin in starCoinPool) {
            if(!eachCoin.activeSelf) {
                eachCoin.transform.position = spawnPosition;
                eachCoin.transform.rotation = Random.rotation;
                eachCoin.SetActive(true);
                eachCoin.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
                eachCoin.GetComponent<ParticleSystem>().Play();
                spawnCount--;
                if (spawnCount == 0) break;
            }
        }
        while (spawnCount > 0) {
            GameObject eachCoin = Instantiate(starCoinPrefab, spawnPosition, Random.rotation, parent: transform);
            starCoinPool.Add(eachCoin);
            eachCoin.SetActive(true);
            eachCoin.GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
            eachCoin.GetComponent<ParticleSystem>().Play();
            spawnCount--;
        }
    }

    public void UpdateCoinText(int coin) {
        starCoinUI.text = coin.ToString();
    }
}