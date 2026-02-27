using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    [SerializeField] GameObject ConfirmQuit;
    [SerializeField] GameObject[] statsButtons;
    [SerializeField] GameObject characterInfo;
    [SerializeField] GameObject inventoryPanel;
    [SerializeField] GameObject statsPanel;
    public static MenuManager instance;

    private PlayerStats[] playerStats;
    [SerializeField] Text[] nameText, hpText, levelText, xpText, currentXPText;
    [SerializeField] Slider[] xpSlider;
    [SerializeField] Image[] characterImage;
    [SerializeField] GameObject[] characterPanel;

    [SerializeField] Text statName, statCredits, statHP, statDex, statStr, statDef; 
    [SerializeField] Text statEquipedMeleeWeapon, statEquipedRangeWeapon, statMeleeWeaponDamage, statRangeWeaponDamage;

    [SerializeField] Image characterStatImage;

    [SerializeField] GameObject itemSlotContainer;
    [SerializeField] Transform itemSlotContainerParent;

    public Text itemName, itemDescription;

    public ItemsManager activeItem;

    [SerializeField] GameObject characterChoicePanel;
    public GameObject itemsDescription;
    [SerializeField] Text[] itemsCharacterChoiceNames;

    [SerializeField] TextMeshProUGUI newCreditsUI;
    [SerializeField] TextMeshProUGUI CreditsUI;

    public TextMeshProUGUI instruction;
    public FeedbackAfterCombat rewardsTexts;

    PlayerController player;
    float lastSpeed;

    [SerializeField] TextMeshProUGUI pistolAmmoText;
    [SerializeField] TextMeshProUGUI SMGAmmoText;
    [SerializeField] TextMeshProUGUI shotgunShellText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        player = GameManager.instance.player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && !BattleManager.instance.isBattleActive && !GameManager.instance.chatting && !GameManager.instance.inStore)
        {
            OpenCloseInventory();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && menu.activeInHierarchy && !BattleManager.instance.isBattleActive && !GameManager.instance.chatting && !GameManager.instance.inStore)
        {
            OpenCloseInventory();
        }


        CreditsUI.text = Inventory.instance.credits.ToString();
        pistolAmmoText.text = Inventory.instance.pistolAmmo.ToString();
        SMGAmmoText.text = Inventory.instance.SMGAmmo.ToString();
        shotgunShellText.text = Inventory.instance.shotgunAmmo.ToString();
    }

    public void UpdateStats()
    {
        playerStats = GameManager.instance.GetPlayerStats();

        for(int i = 0; i < playerStats.Length; i++)
            {
            characterPanel[i].SetActive(true);

            nameText[i].text = playerStats[i].playerName;
            hpText[i].text = "PS: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
            levelText[i].text = "Nivel: " + playerStats[i].playerLevel;
            currentXPText[i].text = "EXP Actual: " + playerStats[i].currentXP;

            characterImage[i].sprite = playerStats[i].characterImage;

            xpText[i].text = playerStats[i].currentXP.ToString() + "/" + playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
            xpSlider[i].maxValue = playerStats[i].xpForNextLevel[playerStats[i].playerLevel];
            xpSlider[i].value = playerStats[i].currentXP;
        }
    }

    public void StatsMenu()
    {
        StatsMenuUpdate(0);
        for(int i = 0; i < playerStats.Length; i++)
        {
            statsButtons[i].SetActive(true);
            statsButtons[i].GetComponentInChildren<Text>().text = playerStats[i].playerName;
        }
    }

    public void StatsMenuUpdate(int playerSelectedNumber)
    {
        PlayerStats playerSelected = playerStats[playerSelectedNumber];
        
        statCredits.text = Inventory.instance.credits.ToString();

        statHP.text = playerSelected.currentHP.ToString() + "/" + playerSelected.maxHP;

        statDex.text = playerSelected.dexterity.ToString();
        statStr.text = playerSelected.strength.ToString();
        statDef.text = playerSelected.defence.ToString();

        characterStatImage.sprite = playerSelected.characterImage;

        statEquipedMeleeWeapon.text = playerSelected.equippedMeleeWeaponName;
        statEquipedRangeWeapon.text = playerSelected.equippedRangeWeaponName;

        statMeleeWeaponDamage.text = playerSelected.meleeDamage.ToString();
        statRangeWeaponDamage.text = playerSelected.rangeDamage.ToString();
    }

    public void UpdateItemsInventory()
    {
        foreach(Transform itemSlot in itemSlotContainerParent)
        {
            Destroy(itemSlot.gameObject);
        }

        foreach(ItemsManager item in Inventory.instance.GetItemsList())
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();

            Image itemImage = itemSlot.Find("Item image").GetComponent<Image>();
            itemImage.sprite = item.icon;

            Text itemsAmountText = itemSlot.Find("Amount Text").GetComponent<Text>();
            if (item.amount > 1)
                itemsAmountText.text = item.amount.ToString();
            else
                itemsAmountText.text = "";

            itemSlot.GetComponent<ItemButton>().itemOnButton = item;
        }
    }

    public void DiscardItem()
    {
        Inventory.instance.RemoveItem(activeItem);
        UpdateItemsInventory();
    }

    public void UseItem(int selectedCharacter)
    {
        activeItem.UseItem(selectedCharacter);
        DiscardItem();
    }

    public void OpenCloseInventory()
    {
        if (menu.activeInHierarchy)
        {
            if (ConfirmQuit.activeInHierarchy)
                ConfirmQuit.SetActive(false);
            menu.SetActive(false);
            player.moveSpeed = lastSpeed;   
        }
        else
        {
            lastSpeed = player.moveSpeed;
            player.moveSpeed = 0;
            UpdateStats();
            menu.SetActive(true);
            inventoryPanel.SetActive(false);
            statsPanel.SetActive(false);
            characterInfo.SetActive(true);
        }
    }

    public void AddCreditsUI()
    {
        int creditsToGive = Random.Range(5, 20);
        newCreditsUI.gameObject.SetActive(true);
        newCreditsUI.text = "+" + creditsToGive.ToString();
        newCreditsUI.gameObject.SetActive(false);
        Inventory.instance.AddCredits(creditsToGive);
        StartCoroutine(rewardsTexts.ShowNewCredits(creditsToGive.ToString()));
    }
}
