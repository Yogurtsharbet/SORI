using System;
using UnityEngine;
using UnityEngine.UI;

enum CinematicType {
    Intro, Forest, Ruins, Butterfly01, Butterfly02, Butterfly03
}

public class PlayerBehavior : MonoBehaviour {
    private PlayerInputActions playerInputAction;
    private StarCoinManager starCoinManager;
    private Animator playerAnimator;

    public delegate void PlayerDie();
    public static event PlayerDie OnPlayerDie;

    private float playerMaxHP;
    private float playerHP;
    [SerializeField] private int starCoin;
    private bool isCombineMode;

    [SerializeField] private Image playerHPSlider;
    private Animator butterflyAnimator;

    public int StarCoin { get { return starCoin; } }
    public bool IsCombineMode { get { return isCombineMode; } }

    private bool[] isWatchedCinematic = new bool[Enum.GetValues(typeof(CinematicType)).Length];

    private void Awake() {
        starCoinManager = GameManager.Instance.starCoinManager;
        playerInputAction = new PlayerInputActions();
        playerAnimator = GetComponent<Animator>();

        butterflyAnimator = GameObject.FindGameObjectWithTag("Butterfly")?.GetComponent<Animator>();
    }

    private void Start() {
        //TODO: Save 구현시 연동
        playerMaxHP = 100f;
        playerHP = GameManager.Instance.isCompleteTutorial ? PlayerPrefs.GetFloat("PlayerHP") : playerMaxHP;
        starCoin = GameManager.Instance.isCompleteTutorial ? PlayerPrefs.GetInt("StarCoin") : 0;
        starCoinManager.UpdateCoinText(starCoin);
        isWatchedCinematic[(int)CinematicType.Intro] = true;

        if (GameManager.Instance.isCompleteTutorial) {
            for (int i = 0; i < isWatchedCinematic.Length; i++)
                isWatchedCinematic[i] = true;
            if (GameManager.Instance.currentScene == "Map") {
                transform.position = CameraControl.Instance.RuinsPosition.position;
                GameManager.Instance.AfterCompleteStage();
            }
        }

    }

    private void OnEnable() {
        playerInputAction.Enable();
        OnPlayerDie += PlayerDieAnimation;
    }

    private void OnDisable() {
        playerInputAction.Disable();
        OnPlayerDie -= PlayerDieAnimation;
    }

    private void OnDestroy() {
        PlayerPrefs.SetInt("StarCoin", starCoin);
        PlayerPrefs.SetFloat("PlayerHP", playerHP);
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


    // GameState 구현에 따른 삭제. 240927
    //private void OnInventory() {
    //    if (invenContainer.gameObject.activeSelf) invenContainer.CloseInventory();
    //    else invenContainer.OpenInventory();
    //}
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
        CheckWater(other);
    }
    private void CheckEarningCoins(Collider coin) {
        if (coin.gameObject.layer == LayerMask.NameToLayer("Coin")) {
            EarnStarCoin();
        }
    }

    public void EarnStarCoin(int coins = -1) {
        if (coins == -1) coins = UnityEngine.Random.Range(10, 50);
        starCoin += coins;
        starCoinManager.UpdateCoinText(starCoin);
    }

    public void UseStarCoin(int coins) {
        starCoin -= coins;
        starCoinManager.UpdateCoinText(starCoin);
    }

    private void CheckCinematicZone(Collider zone) {
        if (zone.gameObject.layer == LayerMask.NameToLayer("CinematicZone")) {
            if (zone.name == "ForestEntrance" && !isWatchedCinematic[(int)CinematicType.Forest]) {
                isWatchedCinematic[(int)CinematicType.Forest] = true;
                CameraControl.Instance.SetCamera("Forest");
            }
            else if (zone.name == "Ruins" && !isWatchedCinematic[(int)CinematicType.Ruins]) {
                isWatchedCinematic[(int)CinematicType.Ruins] = true;
                CameraControl.Instance.SetCamera("Ruins");
            }
            else if (zone.name == "Home" && GameManager.Instance.isCompleteTutorial) {
                Debug.Log("[DEBUG] :: Player call OnTriggerEnter and zone is Home");
                CameraControl.Instance.SetCamera("Home");
            }
            else if (zone.name == "ButterflyZone01" && !isWatchedCinematic[(int)CinematicType.Butterfly01]) {
                isWatchedCinematic[(int)CinematicType.Butterfly01] = true;
                butterflyAnimator.Play("ButterFlyZone01");
            }
            else if (zone.name == "ButterflyZone02" && !isWatchedCinematic[(int)CinematicType.Butterfly02]) {
                isWatchedCinematic[(int)CinematicType.Butterfly02] = true;
                butterflyAnimator.Play("ButterFlyZone02");
            }
            else if (zone.name == "ButterflyZone03" && !isWatchedCinematic[(int)CinematicType.Butterfly03]) {
                isWatchedCinematic[(int)CinematicType.Butterfly03] = true;
                butterflyAnimator.Play("ButterFlyZone03");
            }
        }
    }
    private void CheckWater(Collider water) {
        if (water.gameObject.layer == LayerMask.NameToLayer("Water")) {
            transform.position = new Vector3(475f, 6f, 378f);
            TakeDamage(18f);
        }
    }
}
