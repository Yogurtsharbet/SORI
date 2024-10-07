using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageLoadManager : MonoBehaviour {
    private string sceneName = "StageBase";
    private int clearCount = 1;
    private int stageCount;
    private Vector3 stageOffset = new Vector3(0, 0, 290);

    private void Start() {
        LoadScene();
    }

    public void LoadScene() {
        if (stageCount > clearCount) 
            FindObjectOfType<MainLoading>().StartLoading(1);
        else
            StartCoroutine(Load());
    }

    private IEnumerator Load() {
        stageCount++;
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!op.isDone)
            yield return null;

        var scene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        var rootObjects = scene.GetRootGameObjects();

        rootObjects[0].transform.position = stageOffset * stageCount;

        if (SceneManager.sceneCount > 3) StartCoroutine(Unload(SceneManager.GetSceneAt(0)));
        if (SceneManager.sceneCount > 2) {
            var targetPosition = CameraControl.Instance.CameraBorder.position + stageOffset;
            while(Vector3.Distance(CameraControl.Instance.CameraBorder.position, targetPosition) > 0.1f) {
                CameraControl.Instance.CameraBorder.position = 
                    Vector3.MoveTowards(CameraControl.Instance.CameraBorder.position, targetPosition, Time.deltaTime * 50f);
                yield return null;
            }
            CameraControl.Instance.CameraBorder.position = targetPosition;
        }
    }

    private IEnumerator Unload(Scene scene) {
        var op = SceneManager.UnloadSceneAsync(scene);
        while (!op.isDone)
            yield return null;
    }
}
