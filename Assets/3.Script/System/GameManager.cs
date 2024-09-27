using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour {
    private PlayerInputActions inputAction;

    public IGameState gameState { get; private set; }
    private Dictionary<GameState, IGameState> State;

    private void Awake() {
        InitializeInputAction();
    }

    private void Start() {
        InitializeGameState();
        ChangeState(State[GameState.Normal]);
    }

    public void InitializeInputAction() {
        inputAction = new PlayerInputActions();
        inputAction.UI.Cancel.performed += value => gameState.OnCancel();
        inputAction.UI.Enter.performed += value => gameState.OnEnter();
        inputAction.UI.Tab.performed += value => gameState.OnTab();
        inputAction.UI.Inventory.performed += value => gameState.OnInven();
        inputAction.Enable();
    }
    public void InitializeGameState() {
        State = new Dictionary<GameState, IGameState>();
        State.Add(GameState.Normal, new State_Normal());
        State.Add(GameState.Combine, new State_Combine());
        State.Add(GameState.Select, new State_Select());
        State.Add(GameState.Inven, new State_Inven());
    }

    public void ChangeState(GameState state) {
        if (gameState != State[state]) {
            gameState = State[state];
            gameState.OnStateChanged();
        }
    }

    public void ChangeState(IGameState state) {
        if (gameState != state) {
            gameState = state;
            gameState.OnStateChanged();
        }
    }
}
