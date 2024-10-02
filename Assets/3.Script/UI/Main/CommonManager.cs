using UnityEngine;

public class CommonManager : MonoBehaviour {
    private static CommonManager instance = null;

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
}
