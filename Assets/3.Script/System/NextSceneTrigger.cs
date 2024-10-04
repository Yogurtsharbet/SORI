using UnityEngine;

public class NextSceneTrigger : MonoBehaviour {
    private MainLoading mainLoading;
    [SerializeField] private int sceneIndex;

    private bool isVisited = false;

    private void Awake() {
        mainLoading = FindObjectOfType<MainLoading>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            LoadNextScene();
        }
    }

    private void LoadNextScene() {
        if (sceneIndex == 1 && !isVisited) {
            mainLoading.StartLoading(sceneIndex + 1);
            return;
        }
        //TODO: 모든 씬 예외처리 필요
    }
}
