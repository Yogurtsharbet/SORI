using System;
using UnityEngine;
using UnityEngine.UI;

enum CinematicType {
    Intro, Forest
}

public class PlayerBehavior : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private StarCoinManager starCoinManager;
    private Animator playerAnimator;

    public delegate void PlayerDie();
    public static event PlayerDie OnPlayerDie;

    private float playerMaxHP;
    private float playerHP;
    private int starCoin;
    private bool isCombineMode;

    [SerializeField] private Image playerHPSlider;

    public int StarCoin { get { return starCoin; } }
    public bool IsCombineMode { get { return isCombineMode; } }

    private bool[] isWatchedCinematic = new bool[Enum.GetValues(typeof(CinematicType)).Length];

    private CombineContainer combineContainer;
    private InvenContainer invenContainer;

    private void Awake() {
        starCoinManager = FindObjectOfType<StarCoinManager>();
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();

        combineContainer = FindObjectOfType<CombineContainer>();
        invenContainer = FindObjectOfType<InvenContainer>();
    }

    private void Start() {
        //TODO: Save 구현시 연동
        playerMaxHP = 100f;
        playerHP = playerMaxHP;
        starCoin = 0;
        starCoinManager.UpdateCoinText(starCoin);
        isWatchedCinematic[(int)CinematicType.Intro] = true;
    }

    private void OnEnable() {
        playerInputAction.Enable();
        OnPlayerDie += PlayerDieAnimation;
    }

    private void OnDisable() {
        playerInputAction.Disable();
        OnPlayerDie -= PlayerDieAnimation;
    }

    // GameState 구현에 따른 삭제. 240927.
    //private void OnCombine() {
    //    // *************************************** //
    //    // Toggle Combine Mode when Tab is pressed //
    //    // *************************************** //
    //    if (CameraControl.Instance.cameraStatus == CameraControl.CameraStatus.TopView ||
    //        CameraControl.Instance.cameraStatus == CameraControl.CameraStatus.CombineView)
    //        ToggleCombineMode();
    //    else if (CameraControl.Instance.cameraStatus == CameraControl.CameraStatus.SelectView)
    //        CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectTopView);
    //    else CameraControl.Instance.SetCamera(CameraControl.CameraStatus.SelectView);
    //}

    private void OnInventory() {
        if (invenContainer.gameObject.activeSelf) invenContainer.CloseInventory();
        else invenContainer.OpenInventory();
    }

    // GameState 구현에 따른 삭제. 240927
    //public void ToggleCombineMode() {
    //    isCombineMode = !isCombineMode;

    //    if (IsCombineMode) 
    //        combineContainer.OpenCombineField();
    //    else combineContainer.CloseCombineField();

    //    playerAnimator.SetBool("isCombineMode", isCombineMode);
    //    CameraControl.Instance.ChangePlayerCamera();
    //}

    private void PlayerDieAnimation() {
        playerAnimator.SetTrigger("triggerDie");
    }

    public void TakeDamage(float damage) {
        playerHP -= damage;
        if (playerHP < 0) {
            OnPlayerDie?.Invoke();
        }
        SetSlider(playerHP);
    }

    public void Heal(float heal) {
        playerHP += heal;
        if (playerHP > playerMaxHP)
            playerHP = playerMaxHP;
        SetSlider(playerHP);
    }

    private void SetSlider(float hp) {
        //TODO: Lerp 로 부드럽게 닳는 효과
        playerHPSlider.fillAmount = hp / playerMaxHP;
    }

    private void OnTriggerEnter(Collider other) {
        CheckEarningCoins(other);
        CheckCinematicZone(other);

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

    private void CheckCinematicZone(Collider zone) {
        if (CameraControl.Instance.isDebugging) return;
        //TODO: REMOVE THIS LINE WHEN RELEASE

        if (zone.gameObject.layer == LayerMask.NameToLayer("CinematicZone")) {
            if (zone.name == "ForestEntrance" && !isWatchedCinematic[(int)CinematicType.Forest]) {
                isWatchedCinematic[(int)CinematicType.Forest] = true;
                CameraControl.Instance.SetCamera("Forest");
            }
        }
    }
}
