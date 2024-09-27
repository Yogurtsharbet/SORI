using UnityEngine;

public interface IGameState {
    public void OnStateChanged();
    public void OnEnter();
    public void OnCancel();
    public void OnTab();
    public void OnInven();


}

public enum GameState {
    Normal, Combine, Select, Inven
}

public class State_Normal : IGameState {
    private GameManager gameManager;

    public State_Normal() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void OnStateChanged() {
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.TopView);
    }

    public void OnEnter() {
        return;
    }

    public void OnCancel() {
        //TODO: 일시정지 UI
        throw new System.NotImplementedException();
    }


    public void OnTab() {
        gameManager.ChangeState(GameState.Combine);
    }

    public void OnInven() {
        gameManager.ChangeState(GameState.Inven);
    }
}

public class State_Combine : IGameState {
    private GameManager gameManager;
    private CombineContainer combineContainer;
    private Animator playerAnimator;

    public State_Combine() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        combineContainer = GameObject.FindObjectOfType<CombineContainer>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void OnStateChanged() {
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.CombineView);
        combineContainer.OpenCombineField();
        playerAnimator.SetBool("isCombineMode", true);
    }

    public void OnEnter() {
        return;
    }

    public void OnCancel() {
        gameManager.ChangeState(GameState.Normal);
        combineContainer.CloseCombineField();
        playerAnimator.SetBool("isCombineMode", false);
    }

    public void OnTab() {
        OnCancel();
    }

    public void OnInven() {
        gameManager.ChangeState(GameState.Inven);
        combineContainer.CloseCombineField();
        playerAnimator.SetBool("isCombineMode", false);
    }

    public void OnCombineSubmit() {
        gameManager.ChangeState(GameState.Select);
        combineContainer.CloseCombineField();
    }
}

public class State_Select : IGameState {
    private GameManager gameManager;
    private SelectControl selectControl;
    private Animator playerAnimator;

    public State_Select() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        selectControl = GameObject.FindObjectOfType<SelectControl>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void OnStateChanged() {
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
    }

    public void OnEnter() {
        if (selectControl.IsSelectComplete) {
            gameManager.ChangeState(GameState.Normal);
            selectControl.ActivateSelected();
            playerAnimator.SetBool("isCombineMode", false);
        }
    }

    public void OnCancel() {
        gameManager.ChangeState(GameState.Combine);
    }

    public void OnTab() {
        if (CameraControl.Instance.cameraStatus == CameraControl.CameraStatus.SelectView)
            CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectTopView);
        else CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
    }

    public void OnInven() {
        gameManager.ChangeState(GameState.Inven);
    }
}

public class State_Inven : IGameState {
    private GameManager gameManager;
    private InvenContainer invenContainer;

    public State_Inven() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        invenContainer = GameObject.FindObjectOfType<InvenContainer>();
    }

    public void OnStateChanged() {
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.TopView);
        invenContainer.OpenInventory();
    }

    public void OnEnter() {
        return;
    }

    public void OnCancel() {
        gameManager.ChangeState(GameState.Normal);
        invenContainer.CloseInventory();
    }

    public void OnTab() {
        gameManager.ChangeState(GameState.Combine);
        invenContainer.CloseInventory();
    }

    public void OnInven() {
        OnCancel();
    }
}
