using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TextCore.Text;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance;

    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp,
        UsingInventory,
        OpenChest
    }

    public GameState _CurrentState;
    [SerializeField] GameState _PreviousState;

    [Header("Screens")]
    [SerializeField] GameObject _PauseScreen;
    [SerializeField] GameObject _ResultScreen;
    [SerializeField] GameObject _LevelUpScreen;
    [SerializeField] GameObject _InventoryScreen;
    [SerializeField] Transform _InventoryOpen;
    [SerializeField] Transform _InventoryClose;

    [Header("Slide LevelUp")]
    [SerializeField] Transform _UnderScreenPos;
    [SerializeField] Transform _OnScreenPos;
    [SerializeField] float _SlideSpeed;
    [SerializeField] GameObject _LevelUPSlide;
    bool _LevelUpSliding;

    [Header("Slide Chest")]
    [SerializeField] GameObject _OpenChestScene;
    [SerializeField] GameObject _OpenChestSlide;
    public TMP_Text _ChestItemName;
    public TMP_Text _ChestItemDescription;
    public Image _ChestItemImageMid;
    public Image _ChestItemImageL;
    public Image _ChestItemImageR;
    public GameObject _Merge;
    bool _SlidingChest;

    [Header("Buttom")]
    [SerializeField] GameObject _InventoryOpenButtom;

    [Header("Current Stat Displays")]
    public TMP_Text _CurrentHealthDisplay;
    public TMP_Text _CurrentShield;
    public TMP_Text _CurrentMoveSpeedBoostDisplay;
    public TMP_Text _CurrentAtkBoostDisplay;

    [Header("Result Screen Display")]
    [SerializeField] Image _ChosenCharacterImage;
    [SerializeField] TMP_Text _ChosenCharacterText;
    [SerializeField] TMP_Text _ReachLevelDisplay;
    [SerializeField] TMP_Text _TimeSurvivalDisplay;
    [SerializeField] TMP_Text _EnemyKillDisplay;
    public int _EnemyKillCount;
    [SerializeField] List<Image> _ChosenWeaponUI = new List<Image>(6);

    public InventoryDisplay[] _Inventory;

    [System.Serializable]
    public class InventoryDisplay
    {
        public Image[] _DisplayInventoryImage;
    }

    [Header("Stopwatch")]
    [HideInInspector] public float _StopwatchTime;
    [SerializeField] TMP_Text _StopwatchDisplay;
    public bool StopTime;

    [Header("Avatar Display")]
    [SerializeField] Image _AvatarDiaplay;

    ChangeMouseCurser _ChangeMouseCurser;

    [HideInInspector] public bool _IsGameOver = false;

    [HideInInspector] public bool _ChosenUpgrade;

    [HideInInspector] public bool _IsAiming = false;
    [HideInInspector] public bool _StopAiming = false;

    [SerializeField] GameObject _PlayerObject;

    float _GameSpeed = 1;

    public SaveData _SaveData;
    int _LastRound;
    CharactorID _CharactorID;

    public int[] _AllExpOrbCollect = new int[3];
    public int[] _AllGemCollect = new int[4];

    public int _AllPotionCollect;
    public int _AllWeaponChestCollect;

    public int[] _Monster = new int[9];
    public int[] _Boss = new int[3];

    public GameObject _InventoryTotorial;
    public bool _FirstTutorial;
    public bool _IsTutorial;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DisabbleScreen();
        _FirstTutorial = _SaveData._FirstTutorial;
    }

    private void Start()
    {
        _ChangeMouseCurser = GetComponent<ChangeMouseCurser>();
    }

    private void Update()
    {
        switch (_CurrentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                CheckForUsingInventory();
                AimingCheck();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!_IsGameOver)
                {
                    _IsGameOver = true;
                }
                break;

            case GameState.LevelUp:
                if (!_ChosenUpgrade)
                {
                    _ChosenUpgrade = true;
                }
                if (_LevelUpSliding)
                {
                    SlideLevelUp();
                }
                break;

            case GameState.UsingInventory:
                CheckForUsingInventory();
                
                break;

            case GameState.OpenChest:
                if (_SlidingChest)
                {
                    SlideChestUI();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    EndChest();
                }
                break;
            default:
                Debug.LogWarning("State Dose Not Exist");
                break;
        }
    }

    public void ChangeState(GameState newState)
    {
        _CurrentState = newState;
    }

    void DisabbleScreen()
    {
        //disable
        _PauseScreen.SetActive(false);
        _ResultScreen.SetActive(false);
        _LevelUpScreen.SetActive(false);
        _OpenChestScene.SetActive(false);
        //move
        _InventoryScreen.transform.position = _InventoryClose.position;
    }

    #region Pause
    public void PauseGame()
    {
        if (_CurrentState != GameState.Paused)
        {
            _PreviousState = _CurrentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0; //stop game
            _PauseScreen.SetActive(true);
            _InventoryOpenButtom.SetActive(false);
            _ChangeMouseCurser.ChangeMouseToNormal();
            _StopAiming = true;
        }
    }
    public void ResumeGame()
    {
        if (_CurrentState == GameState.Paused)
        {
            ChangeState(_PreviousState);
            Time.timeScale = _GameSpeed; //Resume game
            _PauseScreen.SetActive(false);
            _InventoryOpenButtom.SetActive(true);
            if (_IsAiming)
            {
                _ChangeMouseCurser.ChangeMouseToAim();
            }
            else
            {
                _ChangeMouseCurser.ChangeMouseToNormal();
            }
            _StopAiming = false;
        }
    }
    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_CurrentState == GameState.Paused)
                ResumeGame();
            else
                PauseGame();
        }
    }
    #endregion

    #region Inventory
    public void StartUsingInventory()
    {
        _InventoryOpenButtom.SetActive(false);
        ChangeState(GameState.UsingInventory);

        Time.timeScale = 0;
        _ChangeMouseCurser.ChangeMouseToNormal();
        _InventoryScreen.transform.position = _InventoryOpen.position;
        _StopAiming = true;
    }

    public void StopUsingInventory()
    {
        if (_IsTutorial)
        {
            return;
        }

        if (_IsAiming)
        {
            _ChangeMouseCurser.ChangeMouseToAim();
        }
        else
        {
            _ChangeMouseCurser.ChangeMouseToNormal();
        }

        Time.timeScale = _GameSpeed;
        _InventoryScreen.transform.position = _InventoryClose.position;
        _InventoryOpenButtom.SetActive(true);
        ChangeState(GameState.Gameplay);
        _StopAiming = false;
    }

    void CheckForUsingInventory()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_CurrentState == GameState.UsingInventory)
                StopUsingInventory();
            else
                StartUsingInventory();
        }
    }

    public void CheckTutorial()
    {
        if (!_FirstTutorial)
        {
            Time.timeScale = 0;
            _StopAiming = true;
            _ChangeMouseCurser.ChangeMouseToNormal();
            _InventoryTotorial.SetActive(true);
            _IsTutorial = true;
        }
        else
        {
            _InventoryTotorial.SetActive(false);
        }
    }
    #endregion

    #region Gameover & Display
    public void GameOver(float time)
    {
        _SaveData._LastGames.Add(new SaveData.LastGame());
        if (_SaveData._LastGames.Count > 3)
        {
            _SaveData._LastGames.RemoveAt(0);
        }
        _LastRound = _SaveData._LastGames.Count - 1;
        
        _ChangeMouseCurser.ChangeMouseToNormal();
        StartCoroutine(WaitforAnim(time));
    }
    IEnumerator WaitforAnim(float time)
    {
        _TimeSurvivalDisplay.text = _StopwatchDisplay.text;
        yield return new WaitForSeconds(time);
        ChangeState(GameState.GameOver);
        DisplayResult();

        Time.timeScale = 0;
    }
    void DisplayResult()
    {
        
        _ResultScreen.SetActive(true);
        _InventoryOpenButtom.SetActive(false);

        _SaveData._LastGames[_LastRound]._CharactorID = _CharactorID;
        _SaveData._LastGames[_LastRound]._EnemyKill = _EnemyKillCount;
        _SaveData._LastGames[_LastRound]._PlayTime = _StopwatchTime;

        _SaveData._AllTimePLay += _StopwatchTime;
        _SaveData._AllEnemyKill += _EnemyKillCount;

        for (int i = 0; i < _AllExpOrbCollect.Length; i++)
        {
            _SaveData._AllExpOrbCollect[i] += _AllExpOrbCollect[i];
        }

        for (int i = 0; i < _AllGemCollect.Length; i++)
        {
            _SaveData._AllGemCollect[i] += _AllGemCollect[i];
        }
        _SaveData._AllPotionCollect += _AllPotionCollect;
        _SaveData._AllUpgradeChestCollect += _AllWeaponChestCollect;

        for (int i = 0; i < _Monster.Length; i++)
        {
            _SaveData._MonsterKill[i] += _Monster[i];
        }
        for (int i = 0; i < _Boss.Length; i++)
        {
            _SaveData._BossKill[i] += _Boss[i];
        }
    }
    public void AssignChosenCharacterUI(CharacterScriptableObject character)
    {
        _AvatarDiaplay.sprite = character.Icon;
        _ChosenCharacterText.text = character.Name;
        _ChosenCharacterImage.sprite = character.FullBodyImage;
        _CharactorID = character.CharactorID;
    }
    public void AssignLevelReach(int levelReach)
    {
        _ReachLevelDisplay.text = levelReach.ToString();
    }
    public void AssignWeaponUI(List<WeaponController> weapon)
    {
        if(weapon.Count != _ChosenWeaponUI.Count)
        {
            return;
        }

        //show weapon
        for (int i = 0; i < _ChosenWeaponUI.Count; i++)
        {
            if (weapon[i] != null)
            {
                _ChosenWeaponUI[i].enabled = true;
                _ChosenWeaponUI[i].sprite = weapon[i]._WeaponData.Icon;
                _SaveData._LastGames[_LastRound]._WeaponID[i] = weapon[i]._WeaponData.WeaponTypeID;
            }
            else
            {
                _ChosenWeaponUI[i].enabled = false;
                _SaveData._LastGames[_LastRound]._WeaponID[i] = WeaponID.None;
            }
        }
    }
    #endregion

    #region Time
    void UpdateStopwatch()
    {
        if (StopTime)
            return;

        _StopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();
    }
    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(_StopwatchTime / 60);
        int seconds = Mathf.FloorToInt(_StopwatchTime % 60);

        _StopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        _EnemyKillDisplay.text = _EnemyKillCount.ToString();
    }
    #endregion

    #region LevelUp
    public void StartLevelUp()
    {
        StopUsingInventory();
        Time.timeScale = 0;
        // time scale stop on playerStats
        ChangeState(GameState.LevelUp);
        _PlayerObject.SendMessage("RemoveAndApplyUpgrade");
        _InventoryOpenButtom.SetActive(false);
        _LevelUpScreen.SetActive(true);
        _LevelUPSlide.transform.position = _UnderScreenPos.position;
        _LevelUpSliding = true;
    }

    public void EndLevelUp()
    {
        _ChosenUpgrade = false;
        Time.timeScale = _GameSpeed;
        _LevelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
        _InventoryOpenButtom.SetActive(true);
    }

    private void SlideLevelUp()
    {
        _LevelUPSlide.transform.Translate(_SlideSpeed * Vector3.up);
        if(_LevelUPSlide.transform.position.y >= _OnScreenPos.position.y)
        {
            _LevelUpSliding = false;
        }
    }
    #endregion

    #region OpenChest
    public void StartGetChest()
    {
        Time.timeScale = 0;
        ChangeState(GameState.OpenChest);
        _OpenChestScene.SetActive(true);
        _OpenChestSlide.transform.position = _UnderScreenPos.position;
        _SlidingChest = true;
    }

    public void EndChest()
    {
        _OpenChestScene.SetActive(false);
        Time.timeScale = _GameSpeed;
        ChangeState(GameState.Gameplay);
    }

    private void SlideChestUI()
    {
        _OpenChestSlide.transform.Translate(_SlideSpeed * Vector3.up);
        if (_OpenChestSlide.transform.position.y >= _OnScreenPos.position.y)
        {
            _SlidingChest = false;
        }
    }
    #endregion

    public void AimingCheck()
    {
        if (_StopAiming)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            _IsAiming = !_IsAiming;
            if (_IsAiming)
            {
                _ChangeMouseCurser.ChangeMouseToAim();
            }
            else
            {
                _ChangeMouseCurser.ChangeMouseToNormal();
            }
        }
    }

    public void SetGameSpeed(float speed)
    {
        _GameSpeed = speed;
        Time.timeScale = _GameSpeed;
    }
}
