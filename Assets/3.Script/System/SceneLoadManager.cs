using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoadManager : MonoBehaviour {
    private string sceneName = "StageBase";
    private int stageCount;

    public void LoadScene() {
        StartCoroutine(Load());
    }

    private IEnumerator Load() {
        stageCount++;
        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!op.isDone)
            yield return null;

        var scene = SceneManager.GetSceneByName(sceneName);
        var rootObjects = scene.GetRootGameObjects();
        rootObjects[0].transform.position = new Vector3(0, 0, 290 * stageCount);
    }
}
