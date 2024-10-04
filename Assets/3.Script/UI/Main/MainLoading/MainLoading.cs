using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainLoading : MonoBehaviour {
    private Text text;
    private Animator[] loadAni;
    private string[] sceneName = new string[4];

    private void Awake() {
        loadAni = GetComponentsInChildren<Animator>();
        text = GetComponentInChildren<Text>();
        for (int i = 0; i < loadAni.Length; i++) {
            loadAni[i].SetBool("endFadeout", false);
        }
        text.enabled = false;
        setScenceName();
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    private void setScenceName() {
        sceneName[0] = "Main";
        sceneName[1] = "Map";
        sceneName[2] = "StageBase";
        sceneName[3] = "Stage02";
    }

    public void LoadSceneAsync(int index) {
        for (int i = 0; i < loadAni.Length; i++) {
            loadAni[i].SetBool("endFadeout", true);
        }
        text.enabled = true;
        StartCoroutine(LoadSceneCoroutine(index));
    }

    private IEnumerator LoadSceneCoroutine(int index) {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName[index]);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone) {
            if (asyncOperation.progress >= 0.9f) {
                yield return new WaitForSeconds(1f);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void StartLoading(int index) {
        gameObject.SetActive(true);
        FadeControl.Instance.FadeOut();
        StartCoroutine(NewGameDelayedCo(index));
    }

    IEnumerator NewGameDelayedCo(int index) {
        yield return new WaitForSeconds(1.5f);
        LoadSceneAsync(index);
    }
}
