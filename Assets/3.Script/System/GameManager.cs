using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {
    public static GameManager Instance;
    private PlayerInputActions inputAction;

    public StarCoinManager starCoinManager { get; private set; }
    public SelectControl selectControl { get; private set; }
    public CursorControl cursorControl { get; private set; }
    public WordData wordData { get; private set; }
    public FrameActivate frameActivate { get; private set; }
    public FrameValidity frameValidity { get; private set; }
    public WordFunction wordFunction { get; private set; }
    public SceneLoadManager sceneLoadManager { get; private set; }

    public bool isCompleteTutorial;

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
    }

    private void Start() {
        InitializeGameState();
        ChangeState(State[GameState.Normal]);
    }

    private void InitializeGameManager() {
        starCoinManager = GetComponent<StarCoinManager>();
        selectControl = GetComponent<SelectControl>();
        cursorControl = GetComponent<CursorControl>();
        wordData = GetComponent<WordData>();
        frameActivate = GetComponent<FrameActivate>();
        frameValidity = GetComponent<FrameValidity>();
        wordFunction = GetComponent<WordFunction>();
        sceneLoadManager = GetComponent<SceneLoadManager>();
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
    }
}
