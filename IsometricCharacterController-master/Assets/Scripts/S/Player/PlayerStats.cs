using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    CharacterScriptableObject characterData;
    public float currentHealth;
    public float currentRecovery;
    public float currentMoveSpeed;
    public float currentMight;
    public float currentProjectileSpeed;
    public float  currentMagnet;
    public List<GameObject> weaponStorage = new List<GameObject>(4);
    #region Current States Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "HEALTH: " + currentHealth;
                }
            }
        }
    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "LIFE REGEN: " + currentRecovery;
                }
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "MOVE SPEED: " + currentMoveSpeed;
                }
            }
        }
    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "DAMAGE: " + currentMight;
                }
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "PROJECTILE SPEED " + currentProjectileSpeed;
                }
            }
        }
    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "PICKUP RADIUS " + currentMagnet;
                }
            }
        }
    }
    #endregion

    [Header("Experience/level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }
    [Header("I-Frames")]
    public float invincibilityDuration;
    float invicibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges;

    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;
    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;

    public GameObject secondWeapon;
    public GameObject firstPassiveItem;
    public GameObject secondPassiveItem;
    public GameObject bulletSpawnPoint;
    void Awake()
    {
        characterData=CharacterSelector.GetData();
       CharacterSelector.instance.DestroySingleton();
        inventory = GetComponent<InventoryManager>();
       CurrentHealth = characterData.MaxHealth;
       CurrentRecovery = characterData.Recovery;
       CurrentMoveSpeed = characterData.MoveSpeed;
       CurrentProjectileSpeed = characterData.ProjectileSpeed;
       CurrentMight = characterData.Might;
       CurrentMagnet=characterData.Magnet;
        SpawnWeapon(characterData.StartingWeapon);
        //SpawnWeapon(secondWeapon);
        //SpawnPassiveItem(firstPassiveItem);
        //SpawnPassiveItem(secondPassiveItem);
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;
        GameManager.instance.currentHealthDisplay.text = "HEALTH: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "LIFE REGEN: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "MOVE SPEED: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "DAMAGE: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "PROJECTILE SPEED " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "PICKUP RADIUS " + currentMagnet;
        GameManager.instance.AssignChosenCharacterUI(characterData);
        UpdateExpBar();
        UpdateLevelText();
        UpdateHealthBar();
    }
    void Update(){
        if(invicibilityTimer>0){
            invicibilityTimer-=Time.deltaTime;
        }
        else if(isInvincible){
            isInvincible=false;
        }
        Recover();
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
        UpdateExpBar();
    }
    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;
            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;
            UpdateLevelText();
            GameManager.instance.StartLevelUp();
        }
    }

    void UpdateExpBar()
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }
    void UpdateLevelText()
    {
        levelText.text = "LV " + level.ToString();
    }
    public void TakeDamage(float dmg){
        if(!isInvincible){
            CurrentHealth-=dmg;
            invicibilityTimer=invincibilityDuration;
            isInvincible=true;
            if(CurrentHealth<=0){
                Kill();
            }
            UpdateHealthBar();

        }
    }
    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth;
    }
    public void Kill(){
        if(!GameManager.instance.isGameOver){
            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignchosenWeaponAndPassiveItemsUI(inventory.weaponUISlots,inventory.passiveItemUISlots);
            Debug.Log(level);
            GameManager.instance.GameOver();
        }
    }
    public void Resotorehealth(float amount){
        if(CurrentHealth<characterData.MaxHealth){
        CurrentHealth+=amount;
        }
         if(CurrentHealth>characterData.MaxHealth){
        CurrentHealth=characterData.MaxHealth;
        }
    }
   
    void Recover(){
        if(CurrentHealth<characterData.MaxHealth){
            CurrentHealth+=CurrentRecovery*Time.deltaTime;
        }
        if(CurrentHealth>characterData.MaxHealth){
            CurrentHealth=characterData.MaxHealth;
        }
    }
    public void SpawnWeapon(GameObject weapon){
        if (weaponIndex >= inventory.weaponSlots.Count - 1)
        {
            Debug.LogError("Inventory slots full");
        }
        Vector3 spawnPosition = weaponStorage[weaponIndex].transform.position;
        GameObject spawnedWeapon = Instantiate(weapon, spawnPosition, Quaternion.identity);
        spawnedWeapon.transform.SetParent(weaponStorage[weaponIndex].transform);
        inventory.AddWeapon(weaponIndex,spawnedWeapon.GetComponent<WeaponController>());
        weaponIndex++;
    }
    public void SpawnPassiveItem(GameObject passiveItem)
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count - 1)
        {
            Debug.LogError("Iventory slots full");
        }
     
        Vector3 spawnPosition = transform.position + transform.forward + transform.position;
        GameObject spawnedPassiveItem = Instantiate(passiveItem, spawnPosition, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>());
        passiveItemIndex++;
    }

}


