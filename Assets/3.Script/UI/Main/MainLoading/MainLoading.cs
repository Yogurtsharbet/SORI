using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLoading : MonoBehaviour {
    private Text text;
    private Animator[] loadAni;

    private void Awake() {
        loadAni = GetComponentsInChildren<Animator>();
        text = GetComponentInChildren<Text>();
        for (int i = 0; i < loadAni.Length; i++) {
            loadAni[i].SetBool("endFadeout", false);
        }
        text.enabled = false;
    }

    public void LoadSceneAsync() {
        for (int i = 0; i < loadAni.Length; i++) {
            loadAni[i].SetBool("endFadeout", true);
        }
        text.enabled = true;
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine() {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map");

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone) {
            if (asyncOperation.progress >= 0.9f) {
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
