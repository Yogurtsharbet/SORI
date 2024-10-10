using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Subsystems;
using UnityEngine.UIElements;

public class StarCoinControl : MonoBehaviour {
    private Collider starCoinCollider;
    private Rigidbody starCoinRigid;
    private GameObject player;
    private Vector3 playerPosition;

    private float existedTime = 0f;
    private float delayedPickTime { get { return Random.Range(2f, 3f); } }
    private bool pickCoin = false;

    private void Awake() {
        starCoinCollider = GetComponent<Collider>();
        starCoinRigid = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        debugControl = FindObjectOfType<DebugControl>();
    }

    private void OnEnable() {
        starCoinCollider.isTrigger = false;
        starCoinRigid.useGravity = true;
        existedTime = 0;
        StartCoroutine(DelayedPickCoin());
    }

    private DebugControl debugControl;
    private void Update() {
        PickCoin();

        debugControl?.SetText(transform.position + " / " + starCoinCollider.isTrigger);
    }

    private IEnumerator DelayedPickCoin() {
        yield return new WaitForSeconds(delayedPickTime);

        starCoinRigid.velocity = Vector3.zero;
        starCoinRigid.useGravity = false;
        starCoinCollider.isTrigger = true;

        //Vector3 flyDirection = playerPosition + Random.rotation * Vector3.one * Random.Range(1f, 3f);
        //Mathf.Clamp(flyDirection.y, playerPosition.y + 3f, playerPosition.y + 5f);
        //flyDirection = (flyDirection - transform.position).normalized;
        //starCoinRigid.AddForce(flyDirection * 2f, ForceMode.Impulse);
        //starCoinRigid.AddForce(Vector3.up, ForceMode.Impulse);

        //yield return new WaitForSeconds(0.2f);
        pickCoin = true;
    }

    private void PickCoin() {
        if (!pickCoin) return;
        existedTime += Time.deltaTime;
        if (existedTime > 3f) {
            existedTime = 0f;
            starCoinRigid.velocity = Vector3.zero;
        }
        if (playerPosition == null) Destroy(gameObject);
        playerPosition = player.transform.position;
        playerPosition.y += 2f;
        starCoinRigid.MovePosition(Vector3.Slerp(transform.position, playerPosition, Mathf.Pow(existedTime / 3f, 2)));

        // 플레이어에 도달했는지 확인
        if (Vector3.Distance(transform.position, playerPosition) < 0.2f) {
            gameObject.SetActive(false);
            pickCoin = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.parent != null)
            if (other.transform.parent.CompareTag("Player")) {
                gameObject.SetActive(false);
                pickCoin = false;
            }
    }
    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.parent != null)
            if (collision.transform.parent.CompareTag("Player")) {
                gameObject.SetActive(false);
                pickCoin = false;
            }
    }
} 
