using UnityEngine;

public class CommonManager : MonoBehaviour {
    private static CommonManager instance = null;
    [SerializeField] private Material daySkybox;

    public static CommonManager Instance {
        get {
            if (instance == null) {
                return null;
            }
            return instance;
        }
    }

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        var directionalLight = GetComponentInChildren<Light>();
        directionalLight.color = new Color(1f, 0.956f, 0.839f);
        RenderSettings.ambientLight = new Color(0.90f, 0.76f, 0.63f);
        RenderSettings.skybox = daySkybox;
    }
}
