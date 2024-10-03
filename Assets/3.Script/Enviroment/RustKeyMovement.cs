using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class RustKeyMovement : MonoBehaviour {
    private float defaultY;
    private Transform playerTransform;
    private Vector3 keyPosition;

    private void Start() {
        defaultY = transform.position.y;
        keyPosition = transform.position;
        StartCoroutine(FloatingKey());
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && playerTransform == null &&
            Vector3.Distance(transform.position, other.transform.position) < 15f) {
            playerTransform = other.transform;
            if (defaultY == transform.position.y) 
                defaultY = transform.position.y - playerTransform.position.y;
            
            keyPosition = playerTransform.position;
            exitTime = Time.time;
        }
        if (other.CompareTag("DOOR")) {
            UnlockDoor(other.transform);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player") &&
            Vector3.Distance(transform.position, other.transform.position) > 15f) {
            keyPosition = playerTransform.position + playerTransform.forward * 5f;
            if (playerTransform.position.y > defaultY) defaultY += playerTransform.position.y;
            exitTime = Time.time;
        }
    }

    private float exitTime = 1f;
    private IEnumerator FloatingKey() {
        while (true) {
            transform.position = Vector3.Slerp(transform.position, keyPosition, Mathf.Pow((Time.time - exitTime) * 0.4f, 2));

            keyPosition.y = defaultY + Mathf.Sin(Time.time);
            transform.Rotate(Vector3.forward * Time.deltaTime * 10f);

            yield return null;
        }
    }

    private void UnlockDoor(Transform target) {
        StopCoroutine(FloatingKey());
        Sequence sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOMove(target.position + target.forward * 2f, 1f))
            .Append(target.parent.DORotate(new Vector3(0, -130, 0), 1))
            .OnComplete(() => gameObject.SetActive(false))
            .Play();
    }
}
