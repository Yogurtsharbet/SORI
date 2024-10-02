using UnityEngine;

public interface IGameState {
    public void OnStateChanged();
    public void OnEnter();
    public void OnCancel();
    public void OnTab();
    public void OnInven();
}

public enum GameState {
    Normal, Combine, Select, Inven, Shop
}

public class State_Normal : IGameState {
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

    public State_Select() {
        selectControl = GameManager.Instance.selectControl;
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public void OnStateChanged() {
        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
    }

    public void OnEnter() {
        if (selectControl.IsSelectComplete) {
            GameManager.Instance.ChangeState(GameState.Normal);
            selectControl.ActivateSelected();
            playerAnimator.SetBool("isCombineMode", false);
        }
    }

    public void OnCancel() {
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

    public State_Shop() {
        shopContainer = GameObject.FindObjectOfType<ShopContainer>();
    }

    public void OnStateChanged() {
        shopContainer.OpenShopContainer();
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
