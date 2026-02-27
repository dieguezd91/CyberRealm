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

    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;

    public Text ItemName => itemName;
    public Text ItemDescription => itemDescription;

    public ItemManager activeItem;

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
        player = GameManager.instance.Player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && !BattleManager.instance.IsBattleActive && !GameManager.instance.Chatting && !GameManager.instance.InStore)
        {
            OpenCloseInventory();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && menu.activeInHierarchy && !BattleManager.instance.IsBattleActive && !GameManager.instance.Chatting && !GameManager.instance.InStore)
        {
            OpenCloseInventory();
        }


        CreditsUI.text = Inventory.instance.Credits.ToString();
        pistolAmmoText.text = Inventory.instance.PistolAmmo.ToString();
        SMGAmmoText.text = Inventory.instance.SmgAmmo.ToString();
        shotgunShellText.text = Inventory.instance.ShotgunAmmo.ToString();
    }

    public void UpdateStats()
    {
        playerStats = GameManager.instance.GetPlayerStats();

        for(int i = 0; i < playerStats.Length; i++)
            {
            characterPanel[i].SetActive(true);

            nameText[i].text = playerStats[i].PlayerName;
            hpText[i].text = "PS: " + playerStats[i].CurrentHealth + "/" + playerStats[i].MaxHealth;
            levelText[i].text = "Nivel: " + playerStats[i].PlayerLevel;
            currentXPText[i].text = "EXP Actual: " + playerStats[i].CurrentXp;

            characterImage[i].sprite = playerStats[i].CharacterImage;

            xpText[i].text = playerStats[i].CurrentXp.ToString() + "/" + playerStats[i].XpForNextLevel[playerStats[i].PlayerLevel];
            xpSlider[i].maxValue = playerStats[i].XpForNextLevel[playerStats[i].PlayerLevel];
            xpSlider[i].value = playerStats[i].CurrentXp;
        }
    }

    public void StatsMenu()
    {
        StatsMenuUpdate(0);
        for(int i = 0; i < playerStats.Length; i++)
        {
            statsButtons[i].SetActive(true);
            statsButtons[i].GetComponentInChildren<Text>().text = playerStats[i].PlayerName;
        }
    }

    public void StatsMenuUpdate(int playerSelectedNumber)
    {
        PlayerStats playerSelected = playerStats[playerSelectedNumber];
        
        statCredits.text = Inventory.instance.Credits.ToString();

        statHP.text = playerSelected.CurrentHealth.ToString() + "/" + playerSelected.MaxHealth;

        statDex.text = playerSelected.Dexterity.ToString();
        statStr.text = playerSelected.Strength.ToString();
        statDef.text = playerSelected.Defence.ToString();

        characterStatImage.sprite = playerSelected.CharacterImage;

        statEquipedMeleeWeapon.text = playerSelected.EquippedMeleeWeaponName;
        statEquipedRangeWeapon.text = playerSelected.EquippedRangeWeaponName;

        statMeleeWeaponDamage.text = playerSelected.MeleeDamage.ToString();
        statRangeWeaponDamage.text = playerSelected.RangeDamage.ToString();
    }

    public void UpdateItemsInventory()
    {
        foreach(Transform itemSlot in itemSlotContainerParent)
        {
            Destroy(itemSlot.gameObject);
        }

        foreach(ItemManager item in Inventory.instance.GetItemsList())
        {
            RectTransform itemSlot = Instantiate(itemSlotContainer, itemSlotContainerParent).GetComponent<RectTransform>();

            Image itemImage = itemSlot.Find("Item image").GetComponent<Image>();
            itemImage.sprite = item.Icon;

            Text itemsAmountText = itemSlot.Find("Amount Text").GetComponent<Text>();
            if (item.Amount > 1)
                itemsAmountText.text = item.Amount.ToString();
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
            player.MoveSpeed = lastSpeed;   
        }
        else
        {
            lastSpeed = player.MoveSpeed;
            player.MoveSpeed = 0;
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
