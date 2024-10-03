using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamingSceneLoad : MonoBehaviour {
    private SceneLoadManager sceneManager;
    private bool isLoaded;

    private void Awake() {
        sceneManager = GameManager.Instance.sceneLoadManager;
    }

    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")) {
            var direction = Vector3.Angle(transform.forward, other.transform.position - transform.position);
            if (direction < 90f && !isLoaded) {
                isLoaded = true;
                
                sceneManager.LoadScene();
            }
        }
    }
}