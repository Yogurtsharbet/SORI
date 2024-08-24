using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 항아리, 상자 등에서 코인이 나오게 하기.
// 사용법 :
//      FindObjectOfType<CoinDropControl>().SpawnCoin(transform.position);

public class CoinDropControl : MonoBehaviour {
    [SerializeField] private GameObject starCoinPrefab;
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
}
