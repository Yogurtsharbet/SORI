using System.Collections;
using UnityEngine;

public class NextSceneTrigger : MonoBehaviour {
    private MainLoading mainLoading;
    [SerializeField] private int sceneIndex;
    private GameObject gameCanvas;

    private bool isVisited = false;

    private void Awake() {
        mainLoading = FindObjectOfType<MainLoading>();
        gameCanvas = FindObjectOfType<PauseContainer>().gameObject.GetComponentInParent<Canvas>().gameObject;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            StartCoroutine(WaitForFade());
            
        }
    }

    private IEnumerator WaitForFade() {
        var fadeControl = FindObjectOfType<FadeControl>();

        fadeControl.FadeOut();
        while (fadeControl.screenColor.a < 1) 
            yield return null; 
        
        Destroy(FindObjectOfType<GameManager>()?.gameObject);
        Destroy(FindObjectOfType<CameraControl>()?.gameObject);
        LoadNextScene();
    }

    private void LoadNextScene() {
        if (sceneIndex == 1 && !isVisited) {
            mainLoading.StartLoading(sceneIndex + 1);
            gameCanvas.SetActive(false);
            return;
        }
        //TODO: 모든 씬 예외처리 필요
    }
}
