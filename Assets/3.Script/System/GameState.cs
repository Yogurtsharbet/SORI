using UnityEngine;

public interface IGameState {
    public void OnStateChanged();
    public void OnEnter();
    public void OnCancel();
    public void OnTab();
    public void OnInven();
}

public enum GameState {
    Normal, Combine, Select, Inven, Shop, Pause
}

public class State_Normal : IGameState {
    private InteractionController interactionController;

    public State_Normal() {
        InitState();
    }

    private void InitState() {
        interactionController = GameObject.FindObjectOfType<InteractionController>();
    }

    public void OnStateChanged() {
        if (interactionController == null) InitState();
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.TopView);
        GameManager.Instance.selectControl.UnselectAll();
    }

    public void OnEnter() {
        interactionController.OpenInteraction();
        interactionController.ResetType();
    }

    public void OnCancel() {
        GameManager.Instance.ChangeState(GameState.Pause);
    }

    public void OnTab() {
        GameManager.Instance.ChangeState(GameState.Combine);
    }

    public void OnInven() {
        GameManager.Instance.ChangeState(GameState.Inven);
    }
}

public class State_Combine : IGameState {
    private CombineContainer combineContainer;
    private Animator playerAnimator;

    public State_Combine() {
        InitState();
    }
    private void InitState() {
        combineContainer = GameObject.FindObjectOfType<CombineContainer>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void OnStateChanged() {
        if (playerAnimator == null) InitState();
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.CombineView);
        combineContainer.OpenCombineField();
        playerAnimator.SetBool("isCombineMode", true);
    }

    public void OnEnter() {
        return;
    }

    public void OnCancel() {
        GameManager.Instance.ChangeState(GameState.Normal);
        combineContainer.CloseCombineField();
        playerAnimator.SetBool("isCombineMode", false);
    }

    public void OnTab() {
        OnCancel();
    }

    public void OnInven() {
        GameManager.Instance.ChangeState(GameState.Inven);
        combineContainer.CloseCombineField();
        playerAnimator.SetBool("isCombineMode", false);
    }

    public void OnCombineSubmit() {
        GameManager.Instance.ChangeState(GameState.Select);
        combineContainer.CloseCombineField();
    }
}

public class State_Select : IGameState {
    private SelectControl selectControl;
    private Animator playerAnimator;
    private CombineManager combineManager;
    private GameObject CinematicRocks;

    public State_Select() {
        InitState();
    }

    private void InitState() {
        selectControl = GameManager.Instance.selectControl;
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        combineManager = GameObject.FindObjectOfType<CombineManager>();
        CinematicRocks = GameObject.Find("CinematicRocks");
    }

    public void OnStateChanged() {
        if (playerAnimator == null) InitState();
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
    }

    public void OnEnter() {
        if (selectControl.IsSelectComplete) {
            selectControl.ActivateSelected();
            playerAnimator.SetBool("isCombineMode", false);
            if (combineManager.TempFrame != null) {
                combineManager.TempFrameResetWordUse();
            }
            if (GameManager.Instance.currentScene == "Map" &&
                CinematicRocks != null) {
                var colliders = CinematicRocks.GetComponents<Collider>();
                foreach (Collider collider in colliders)
                    collider.enabled = false;
            }
            GameManager.Instance.ChangeState(GameState.Normal);
        }
    }

    public void OnCancel() {
        if (combineManager.TempFrame != null) {
            combineManager.TempResetAndAddList();
        }
        GameManager.Instance.ChangeState(GameState.Combine);
    }

    public void OnTab() {
        if (CameraControl.Instance.cameraStatus == CameraControl.CameraStatus.SelectView)
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectTopView);
        else CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
    }

    public void OnInven() {
        GameManager.Instance.ChangeState(GameState.Inven);
    }
}

public class State_Inven : IGameState {
    private InvenContainer invenContainer;

    public State_Inven() {
        InitState();
    }

    private void InitState() {
        invenContainer = GameObject.FindObjectOfType<InvenContainer>();
    }

    public void OnStateChanged() {
        if (invenContainer == null) InitState();
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.TopView);
        invenContainer.OpenInventory();
    }

    public void OnEnter() {
        return;
    }

    public void OnCancel() {
        GameManager.Instance.ChangeState(GameState.Normal);
        invenContainer.CloseInventory();
    }

    public void OnTab() {
        GameManager.Instance.ChangeState(GameState.Combine);
        invenContainer.CloseInventory();
    }

    public void OnInven() {
        OnCancel();
    }


}
public class State_Shop : IGameState {
    private ShopContainer shopContainer;
    private Animator playerAnimator;

    public State_Shop() {
        InitState();
    }
    private void InitState() {
        shopContainer = GameObject.FindObjectOfType<ShopContainer>();
        playerAnimator = GameObject.FindObjectOfType<PlayerBehavior>().GetComponent<Animator>();
    }

    public void OnStateChanged() {
        if (playerAnimator == null) InitState();
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.CombineView);
        shopContainer.OpenShopContainer();
        playerAnimator.SetFloat("MoveSpeed", 0);
    }

    public void OnCancel() {
        GameManager.Instance.ChangeState(GameState.Normal);
        shopContainer.CloseShopContainer();
    }

    public void OnEnter() {
        return;
    }

    public void OnInven() {
        return;
    }

    public void OnTab() {
        return;
    }
}

public class State_Pause : IGameState {
    private PauseContainer pauseContainer;

    public State_Pause() {
        InitState();
    }

    private void InitState() { 
        pauseContainer = GameObject.FindObjectOfType<PauseContainer>();
    }

    public void OnStateChanged() {
        if (pauseContainer == null) InitState();
        pauseContainer.OpenPause();
    }

    public void OnCancel() {
        GameManager.Instance.ChangeState(GameState.Normal);
    }

    public void OnEnter() {
        //TODO: 아마? 선택된 옵션키 누르기 버튼?
        return;
    }

    public void OnInven() {
        return;
    }

    public void OnTab() {
        return;
    }
}
