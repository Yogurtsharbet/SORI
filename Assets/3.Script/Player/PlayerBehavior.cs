using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private CameraControl cameraControl;
    private Animator playerAnimator;

    private float playerMaxHP;
    private float playerHP;
    private int starCoin;
    private bool isCombineMode;

    public int StarCoin { get { return StarCoin; } }
    public bool IsCombineMode { get { return isCombineMode; } }


    private void Awake() {
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();

        playerInputAction.PlayerActions.Combine.performed += value => OnCombine();
    }

    private void Start() {
        playerMaxHP = 100f;
        playerHP = playerMaxHP;
        starCoin = 0;
    }

    public void TakeDamage (float damage) {
        playerHP -= damage;
        if(playerHP < 0) {
            //TODO: 플레이어 사망
        }
    }

    private void OnEnable() {
        cameraControl = FindObjectOfType<CameraControl>();
        playerInputAction.Enable();
    }

    private void OnDisable() {
        playerInputAction.Disable();
    }

    private void OnCombine() {
        CombineMode();
    }

    private void CombineMode() {
        isCombineMode = !isCombineMode;
        playerAnimator.SetBool("isCombineMode", isCombineMode);
        cameraControl.ChangeCamera();
    }

    private void OnTriggerEnter(Collider other) {
        CheckEarningCoins(other);
    }

    private void CheckEarningCoins(Collider coin) {
        if (coin.gameObject.layer == LayerMask.NameToLayer("Coin")) {
            EarnStarCoin();
        }
    }

    public void EarnStarCoin(int coins = 1) {
        starCoin += coins;
    }

    public void UseStarCoin(int coins) {
        starCoin -= coins;
    }
}
