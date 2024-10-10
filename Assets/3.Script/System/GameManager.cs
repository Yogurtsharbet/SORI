using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using static CameraControl;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    private PlayerInputActions inputAction;
    public WordCardSelectContainer wordCardContainer { get; private set; }
    private QuestController questController;
    [SerializeField] private Material nightSkybox;

    public StarCoinManager starCoinManager { get; private set; }
    public SelectControl selectControl { get; private set; }
    public CursorControl cursorControl { get; private set; }
    public WordData wordData { get; private set; }
    public FrameActivate frameActivate { get; private set; }
    public FrameValidity frameValidity { get; private set; }
    public WordFunction wordFunction { get; private set; }
    public StageLoadManager stageLoadManager { get; private set; }

    public bool isCompleteTutorial;
    public string currentScene { get { return SceneManager.GetActiveScene().name; } }
    public bool isEndAdv = false;

    public IGameState gameState { get; private set; }
    private Dictionary<GameState, IGameState> State;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        InitializeGameManager();
        InitializeInputAction();

        wordCardContainer = FindObjectOfType<WordCardSelectContainer>();
        questController = FindObjectOfType<QuestController>();
    }

    private void Start() {
        InitializeGameState();
        ChangeState(State[GameState.Normal]);
        if (SceneManager.GetActiveScene().name == "Map") {
            var lights = FindObjectsOfType<Light>();
            foreach (var light in lights) {
                if (light.transform.parent.name.Contains("Lantern"))
                    light.enabled = isCompleteTutorial;
            }
        }
        else {
            isCompleteTutorial = true;
        }
    }

    private void OnDisable() {
        inputAction.UI.Cancel.performed -= value => gameState.OnCancel();
        inputAction.UI.Enter.performed -= value => gameState.OnEnter();
        inputAction.UI.Tab.performed -= value => gameState.OnTab();
        inputAction.UI.Inventory.performed -= value => gameState.OnInven();
        inputAction.Disable();
    }

    private void InitializeGameManager() {
        starCoinManager = FindObjectOfType<StarCoinManager>();
        selectControl = GetComponent<SelectControl>();
        cursorControl = GetComponent<CursorControl>();
        wordData = GetComponent<WordData>();
        frameActivate = GetComponent<FrameActivate>();
        frameValidity = GetComponent<FrameValidity>();
        wordFunction = GetComponent<WordFunction>();
        stageLoadManager = GetComponent<StageLoadManager>();
    }

    private void InitializeInputAction() {
        inputAction = new PlayerInputActions();
        inputAction.UI.Cancel.performed += value => gameState.OnCancel();
        inputAction.UI.Enter.performed += value => gameState.OnEnter();
        inputAction.UI.Tab.performed += value => gameState.OnTab();
        inputAction.UI.Inventory.performed += value => gameState.OnInven();
        inputAction.Enable();
    }

    private void InitializeGameState() {
        State = new Dictionary<GameState, IGameState>();
        State.Add(GameState.Normal, new State_Normal());
        State.Add(GameState.Combine, new State_Combine());
        State.Add(GameState.Select, new State_Select());
        State.Add(GameState.Inven, new State_Inven());
        State.Add(GameState.Shop, new State_Shop());
        State.Add(GameState.Pause, new State_Pause());
    }

    public bool CompareState(GameState state) {
        return gameState == State[state];
    }

    public void ChangeState(GameState state) {
        if (gameState != State[state]) {
            if (state == GameState.Combine && !isCompleteTutorial) return;
            gameState = State[state];
            gameState.OnStateChanged();
        }
    }

    public void ChangeState(IGameState state) {
        if (gameState != state) {
            if (state == State[GameState.Combine] && !isCompleteTutorial) return;
            gameState = state;
            gameState.OnStateChanged();
        }
    }

    public void CompleteTutorial() {
        isCompleteTutorial = true;
        StartCoroutine(WaitRockCinematicEnd());
    }

    private IEnumerator WaitRockCinematicEnd() {
        yield return new WaitUntil(() => CameraControl.Instance.cameraStatus == CameraStatus.TopView);

        questController.SetQuestText("[Tab]을 눌러 문장을 조합해 \n돌을 치우세요!");

        Word[] newWord = new Word[1];
        newWord[0] = Word.GetWord(WordData.Search("ROCK").Key);
        wordCardContainer.GetWordCard(newWord);
    }

    public void AfterCompleteStage() {
        Start();
        isEndAdv = true;
        var directionalLight = FindObjectOfType<CommonManager>().GetComponentInChildren<Light>();
        directionalLight.color = new Color(0.5f, 0.2f, 0f);
        RenderSettings.ambientLight = new Color(0.3f, 0.3f, 0.3f);
        RenderSettings.skybox = nightSkybox;

        var bgm = GetComponentInChildren<BGMManager>();
        bgm.UpdateBGM(3);        bgm.StopBGM();
        bgm.PlayBGM();
        var sfx = FindObjectOfType<SFXManager>();
        sfx.StopBirdSfx();

        var doors = GameObject.FindGameObjectsWithTag("DOOR");
        foreach (var door in doors) {
            if (door.transform.parent.name == "DoorOpen") {
                var target = door.transform.parent;
                StartCoroutine(DoorAnimation(target));
                break;
            }
        }
    }

    private IEnumerator DoorAnimation(Transform target) {
        yield return new WaitForSeconds(2f);
        var targetAngle = Quaternion.Euler(0, -108, 0);
        while (target.eulerAngles.y > -108) {
            target.rotation = Quaternion.Slerp(target.rotation, targetAngle, Time.deltaTime);
            if (Quaternion.Angle(target.rotation, targetAngle) < 0.1f)
                target.rotation = targetAngle;
            yield return null;
        }
    }
}
