using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameManager;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gamplay,
        Paused,
        GameOver,
        LevelUp
    }
    public GameState currentState;
    public GameState previousState;

    [Header("Damage Text Settings")]
    public Canvas damageTextCanvas;
    public float textFontSize = 20;
    public TMP_FontAsset textFont;
    public Camera referenceCamera;

    [Header("Screen")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject levelUpScreen;
    [Header("Current Stats Displays")]
    public TMP_Text currentHealthDisplay;
    public TMP_Text currentRecoveryDisplay;
    public TMP_Text currentMoveSpeedDisplay;
    public TMP_Text currentMightDisplay;
    public TMP_Text currentProjectileSpeedDisplay;
    public TMP_Text currentMagnetDisplay;
    [Header("Result Screen Displays")]
    public TMP_Text chosenCharacterName;
    public Image chosenCharacterImage;
    public TMP_Text levelReachedDisplay;
    public TMP_Text timeSurvivedDisplay;
    public List<Image> chosenWeaponUI = new List<Image>(4);
    public List<Image> chosenPassiveItemUI = new List<Image>(4);
    [Header("Stopwatch")]
    public float timeLimit;
    float stopwatchTime;
    public TMP_Text stopWatchDisplay;

    public bool isGameOver = false;
    public bool chooseUpgrade;

    public GameObject playerObject;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Extra " + this + " Deleted");
        }
        DisableScreens();
    }
    void Update()
    {

        switch (currentState)
        {
            case GameState.Gamplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!chooseUpgrade)
                {
                    chooseUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;
            default:
                Debug.LogWarning("State does not exist");
                break;
        }
    }
     IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, float duration = 1f,float speed = 50f)
    {
        GameObject textObj = new GameObject("Damage Floating Text");
        RectTransform rect = textObj.AddComponent<RectTransform>();
        TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
        tmPro.text = text;
        tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
        tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
        tmPro.fontSize = textFontSize;
        if (textFont) tmPro.font = textFont;
        rect.position = referenceCamera.WorldToScreenPoint(target.position);
        Destroy(textObj, duration);
        textObj.transform.SetParent(instance.damageTextCanvas.transform);
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0;
        float yOffset = 0;
        while (t < duration)
        {
            yield return w;
            t += Time.deltaTime;
            tmPro.color = new Color(tmPro.color.r, tmPro.color.g, tmPro.color.b, 1 - t / duration);
            yOffset += speed * Time.deltaTime;
            rect.position = referenceCamera.WorldToScreenPoint(target.position+new Vector3(0,yOffset));
        }
    }
    public static void GenerateFloatingText(string text,Transform target,float duration=1f,float speed = 1f)
    {
        if (!instance.damageTextCanvas) return;
        if (!instance.referenceCamera) instance.referenceCamera = Camera.main;
        instance.StartCoroutine(instance.GenerateFloatingTextCoroutine(text, target, duration, speed));
    }
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }

    public void Pausegame()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
            Debug.Log("Game paused");
        }
    }
   
    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            ChangeState(previousState);
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
            Debug.Log("Game resumed");
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                Pausegame();
            }
        }
    }
    void DisableScreens()
    {
        resultScreen.SetActive(false);
        pauseScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }
    public void GameOver()
    {
        timeSurvivedDisplay.text = stopWatchDisplay.text;
        ChangeState(GameState.GameOver);
    }
    void DisplayResults()
    {
        resultScreen.SetActive(true);
    }
    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        chosenCharacterImage.sprite = chosenCharacterData.Icon;
        chosenCharacterName.text = chosenCharacterData.name;
    }
    public void AssignLevelReachedUI(int levelReachedData)
    {
        Debug.Log(levelReachedData);
        levelReachedDisplay.text = levelReachedData.ToString();
    }
    public void AssignchosenWeaponAndPassiveItemsUI(List<Image> chosenWeaponData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponData.Count != chosenWeaponUI.Count || chosenPassiveItemsData.Count != chosenPassiveItemUI.Count)
        {
            Debug.Log("two array length doesnt match");
            return;
        }
        for (int ii = 0; ii < chosenWeaponUI.Count; ii++)
        {
            if (chosenWeaponData[ii].sprite)
            {
                chosenWeaponUI[ii].enabled = true;
                chosenWeaponUI[ii].sprite = chosenWeaponData[ii].sprite;
            }
            else
            {
                chosenWeaponUI[ii].enabled = false;
            }
        }
        for (int ii = 0; ii < chosenPassiveItemUI.Count; ii++)
        {
            if (chosenPassiveItemsData[ii].sprite)
            {
                chosenPassiveItemUI[ii].enabled = true;
                chosenPassiveItemUI[ii].sprite = chosenPassiveItemsData[ii].sprite;
            }
            else
            {
                chosenPassiveItemUI[ii].enabled = false;
            }
        }
    }
    void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;
        UpdateStopwatchDisplay();
        if (stopwatchTime >= timeLimit)
        {
            playerObject.SendMessage("Kill");
        }
    }
    void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);
        stopWatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        playerObject.SendMessage("RemoveAndApplyUpgrades");
    }
    public void EndLevelUp()
    {
        chooseUpgrade = false;
        Time.timeScale=1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gamplay);
    }

}

