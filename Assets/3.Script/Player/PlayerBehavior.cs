using System;
using UnityEngine;

enum CinematicType {
    Intro, Forest
}

public class PlayerBehavior : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private StarCoinManager starCoinManager;
    private Animator playerAnimator;

    private testCombine testCombineObj;
    public delegate void PlayerDie();
    public static event PlayerDie OnPlayerDie;

    private float playerMaxHP;
    private float playerHP;
    private int starCoin;
    private bool isCombineMode;

    public int StarCoin { get { return StarCoin; } }
    public bool IsCombineMode { get { return isCombineMode; } }

    private bool[] isWatchedCinematic = new bool[Enum.GetValues(typeof(CinematicType)).Length];

    private void Awake() {
        starCoinManager = FindObjectOfType<StarCoinManager>();
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();

        testCombineObj = FindObjectOfType<testCombine>();

        playerInputAction.PlayerActions.Combine.performed += value => OnCombine();
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

    private void OnCombine() {
        // *************************************** //
        // Toggle Combine Mode when Tab is pressed //
        // *************************************** //
        ToggleCombineMode();
    }

    public void ToggleCombineMode() {
        isCombineMode = !isCombineMode;

        //TODO: 단어조합 UI 지우기
        //Combine UI Set/Unset
        //if (IsCombineMode) testCombineObj.SetCombine();
        //else testCombineObj.UnsetCombine();

        playerAnimator.SetBool("isCombineMode", isCombineMode);
        CameraControl.Instance.ChangePlayerCamera();
    }

    private void PlayerDieAnimation() {
        playerAnimator.SetTrigger("triggerDie");
    }

    public void TakeDamage(float damage) {
        playerHP -= damage;
        if (playerHP < 0) {
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
