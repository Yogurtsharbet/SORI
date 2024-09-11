using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CursorType {
    Default, Loading,
}

public class CursorControl : MonoBehaviour {
    private static CursorControl Instance;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D loadingCursor;

    private void Awake() {
        Instance = this;
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }

    public static void SetCursor(CursorType type) {
        switch (type) {
            default: 
            case CursorType.Default:
                Set(Instance.defaultCursor); break;
            case CursorType.Loading:
                Set(Instance.loadingCursor); break;
        }
    }

    private static void Set(Texture2D cursor) {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
    }
}