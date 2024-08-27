using UnityEngine;

public class PlayerBehavior : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private StarCoinManager starCoinManager;
    private CameraControl cameraControl;
    private Animator playerAnimator;

    public delegate void PlayerDie();
    public static event PlayerDie OnPlayerDie;

    private float playerMaxHP;
    private float playerHP;
    private int starCoin;
    private bool isCombineMode;

    public int StarCoin { get { return StarCoin; } }
    public bool IsCombineMode { get { return isCombineMode; } }


    private void Awake() {
        starCoinManager = FindObjectOfType<StarCoinManager>();
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();

        playerInputAction.PlayerActions.Combine.performed += value => OnCombine();
    }

    private void Start() {
        //TODO: Save 구현시 연동
        playerMaxHP = 100f;
        playerHP = playerMaxHP;
        starCoin = 0;
        starCoinManager.UpdateCoinText(starCoin);
    }

    private void OnEnable() {
        cameraControl = FindObjectOfType<CameraControl>();

        playerInputAction.Enable();
        OnPlayerDie += PlayerDieAnimation;
    }

    private void OnDisable() {
        playerInputAction.Disable();
        OnPlayerDie -= PlayerDieAnimation;
    }

    private void OnCombine() {
        CombineMode();
    }

    private void CombineMode() {
        isCombineMode = !isCombineMode;
        playerAnimator.SetBool("isCombineMode", isCombineMode);
        cameraControl.ChangeCamera();
    }

    private void PlayerDieAnimation() {
        playerAnimator.SetTrigger("triggerDie");
    }

    public void TakeDamage (float damage) {
        playerHP -= damage;
        if(playerHP < 0) {
            OnPlayerDie?.Invoke();
        }
    }

    public void Heal(float heal) {
        playerHP += heal;
        if (playerHP > playerMaxHP) 
            playerHP = playerMaxHP;
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
        starCoinManager.UpdateCoinText(starCoin);
    }

    public void UseStarCoin(int coins) {
        starCoin -= coins;
        starCoinManager.UpdateCoinText(starCoin);
    }
}
