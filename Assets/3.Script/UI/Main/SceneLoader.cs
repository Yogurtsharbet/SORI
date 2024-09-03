using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    public Slider progressBar;

    public void LoadSceneAsync() {
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine() {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Map");

        asyncOperation.allowSceneActivation = false;

        if (progressBar != null) {
            progressBar.gameObject.SetActive(true);  // Show the progress bar
        }

        while (!asyncOperation.isDone) {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            if (progressBar != null) {
                progressBar.value = progress;
            }

            if (asyncOperation.progress >= 0.9f) {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        if (progressBar != null) {
            progressBar.gameObject.SetActive(false);
        }
    }
}
