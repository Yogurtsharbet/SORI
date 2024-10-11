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
        if (SceneManager.GetActiveScene().name != "Map" &&
            SceneManager.GetActiveScene().name != "Stage02")
            gameObject.SetActive(false);
    }

    private void setScenceName() {
        sceneName[0] = "Main";
        sceneName[1] = "Map";
        sceneName[2] = "Stage02";
        sceneName[3] = "StageBase";
    }

    public void LoadSceneAsync(int index) {
        for (int i = 0; i < loadAni.Length; i++) {
            loadAni[i].SetBool("endFadeout", true);
        }
        text.enabled = true;
        StartCoroutine(LoadSceneCoroutine(index));
        Debug.Log("[DEBUG] :: MainLoading component call LoadScene to " + index);
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

        //Map 에서 Stage 로 넘어올때는 실행되지 않음. Stage 에서 Map 으로 넘어갈때만 실행됨.
        Destroy(GameManager.Instance.GetComponentInChildren<CameraControl>().gameObject);
        Destroy(GameManager.Instance.GetComponentInChildren<PlayerBehavior>().gameObject);
        Destroy(GameManager.Instance.transform.GetChild(0).gameObject);
        GameManager.Instance.selectControl.IndicatorControl.SetPlayerTransform();
        Destroy(gameObject);
    }

    public void StartLoading(int index) {
        gameObject.SetActive(true);
        FadeControl.Instance.FadeOut();
        StartCoroutine(NewGameDelayedCo(index));
        Debug.Log("[DEBUG] :: StartLoading() is completed");
    }

    IEnumerator NewGameDelayedCo(int index) {
        yield return new WaitForSeconds(1.5f);
        LoadSceneAsync(index);
        Debug.Log("[DEBUG] :: StartLoading() FadeOut Coroutine is completed");

    }
}
