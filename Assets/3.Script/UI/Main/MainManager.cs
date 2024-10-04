using UnityEngine;
using Cinemachine;
using System.Collections;
using UnityEngine.UI;

// [UI] 메인 - 메인 화면 매니저
public class MainManager : MonoBehaviour {
    private MainTitle mainTitle;
    private MainMenuContainer mainMenu;
    private MainLoading mainLoading;

    private CinemachineBlendListCamera introCamera;

    private void Awake() {
        mainTitle = FindObjectOfType<MainTitle>();
        mainMenu = FindObjectOfType<MainMenuContainer>();
        mainLoading = FindObjectOfType<MainLoading>();

        introCamera = FindObjectOfType<CinemachineBlendListCamera>();
    }

    private void Start() {
        OpenMainTitle();
        StartCoroutine(WaitForIntroCamera());
    }

    private IEnumerator WaitForIntroCamera() {
        var lastCamera = introCamera.ChildCameras[introCamera.ChildCameras.Length - 1];
        mainTitle.CloseMainTitle();
        while (true) {
            yield return null;
            if (introCamera.IsLiveChild(lastCamera)) {
                mainTitle.OpenMainTitle();
                StartCoroutine(TransparentMainTitle());
                break;
            }
        }
    }

    private IEnumerator TransparentMainTitle() {
        var images = mainTitle.gameObject.GetComponentsInChildren<Image>();
        var texts = mainTitle.gameObject.GetComponentsInChildren<Text>();
        float alphaValue = 0;
        while (alphaValue < 1) {
            yield return null;
            alphaValue += Time.deltaTime;

            foreach (var each in images) {
                Color tempColor = each.color;
                tempColor.a = Mathf.Clamp(alphaValue, 0, each.name == "Panel" ? 0.45f : 1);
                each.color = tempColor;
            }
            foreach (var each in texts) {
                Color tempColor = each.color;
                tempColor.a = alphaValue;
                each.color = tempColor;
            }
        }
    }

    public void OpenMainTitle() {
        mainTitle.OpenMainTitle();
        mainMenu.CloseMainMenu();
        mainLoading.gameObject.SetActive(false);
    }

    public void OpenMainMenu() {
        mainTitle.CloseMainTitle();
        mainMenu.OpenMainMenu();
        mainLoading.gameObject.SetActive(false);
    }

    public void OpenLoad() {
        mainTitle.CloseMainTitle();
        mainMenu.CloseMainMenu();
        mainLoading.StartLoading(1);
    }
}
