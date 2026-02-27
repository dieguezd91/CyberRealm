using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    [Header("Player Stats UI")]
    [SerializeField] GameObject[] playerBattleStats;
    [SerializeField] Text[] playersNameText;
    [SerializeField] Slider[] playerHealthSlider;

    [Header("Enemy Stats UI")]
    [SerializeField] GameObject[] enemyBattleStats;
    [SerializeField] Text[] enemysNameText;
    [SerializeField] Slider[] enemyHealthSlider;

    [Header("Inventory / Items UI")]
    public GameObject itemsToUseMenu;
    [SerializeField] GameObject itemSlotContainerPrefab;
    [SerializeField] Transform itemSlotContainerParent;
    [SerializeField] Text itemName;
    [SerializeField] Text itemDescriptionText;
    [SerializeField] GameObject itemDescription;

    [Header("Feedback UI")]
    [SerializeField] TextMeshProUGUI damageReceived;
    [SerializeField] TextMeshProUGUI damageDealt;
    [SerializeField] TextMeshProUGUI log;
    [SerializeField] Text amountOfAmmo;
    [SerializeField] public FeedbackAfterCombat rewardsTexts;

    public void SetLegacyData(GameObject[] pbStats, Text[] pnText, Slider[] phSlider, 
                              GameObject[] ebStats, Text[] enText, Slider[] ehSlider,
                              GameObject itemsMenu, GameObject slotPrefab, Transform slotParent,
                              Text iName, Text iDescText, GameObject iDesc,
                              TextMeshProUGUI dReceived, TextMeshProUGUI dDealt, Text aAmmo, 
                              TextMeshProUGUI dlog, FeedbackAfterCombat rTexts)
    {
        if (playerBattleStats == null || playerBattleStats.Length == 0) playerBattleStats = pbStats;
        if (playersNameText == null || playersNameText.Length == 0) playersNameText = pnText;
        if (playerHealthSlider == null || playerHealthSlider.Length == 0) playerHealthSlider = phSlider;
        if (enemyBattleStats == null || enemyBattleStats.Length == 0) enemyBattleStats = ebStats;
        if (enemysNameText == null || enemysNameText.Length == 0) enemysNameText = enText;
        if (enemyHealthSlider == null || enemyHealthSlider.Length == 0) enemyHealthSlider = ehSlider;
        
        if (itemsToUseMenu == null) itemsToUseMenu = itemsMenu;
        if (itemSlotContainerPrefab == null) itemSlotContainerPrefab = slotPrefab;
        if (itemSlotContainerParent == null) itemSlotContainerParent = slotParent;
        if (itemName == null) itemName = iName;
        if (itemDescriptionText == null) itemDescriptionText = iDescText;
        if (itemDescription == null) itemDescription = iDesc;
        
        if (damageReceived == null) damageReceived = dReceived;
        if (damageDealt == null) damageDealt = dDealt;
        if (amountOfAmmo == null) amountOfAmmo = aAmmo;
        if (log == null) log = dlog;
        if (rewardsTexts == null) rewardsTexts = rTexts;
    }

    public void UpdatePlayerStats(List<BattleCharacters> activeCharacters)
    {
        for (int i = 0; i < playersNameText.Length; i++)
        {
            if (activeCharacters.Count > i && activeCharacters[i].IsPlayer)
            {
                playerBattleStats[i].SetActive(true);
                playersNameText[i].text = activeCharacters[i].CharacterName;
                playerHealthSlider[i].maxValue = activeCharacters[i].MaxHealth;
                playerHealthSlider[i].value = activeCharacters[i].CurrentHealth;
            }
            else
            {
                if (playerBattleStats.Length > i && playerBattleStats[i] != null)
                    playerBattleStats[i].SetActive(false);
            }
        }
    }

    public void UpdateEnemyStats(List<BattleCharacters> activeCharacters)
    {
        for (int i = 0; i < enemysNameText.Length; i++)
        {
            if (activeCharacters.Count > i + 1 && !activeCharacters[i+1].IsPlayer)
            {
                enemyBattleStats[i].SetActive(true);
                enemysNameText[i].text = activeCharacters[i+1].CharacterName;
                enemyHealthSlider[i].maxValue = activeCharacters[i+1].MaxHealth;
                enemyHealthSlider[i].value = activeCharacters[i+1].CurrentHealth;
            }
            else
            {
                if (enemyBattleStats.Length > i && enemyBattleStats[i] != null)
                    enemyBattleStats[i].SetActive(false);
            }
        }
    }

    public void UpdateAmmo(ItemManager equipedRangeWeapon)
    {
        if (equipedRangeWeapon != null)
        {
            switch (equipedRangeWeapon.WeaponType)
            {
                case WeaponType.Escopeta:
                    amountOfAmmo.text = Inventory.instance.ShotgunAmmo.ToString();                    
                    break;
                case WeaponType.Subfusil:
                    amountOfAmmo.text = Inventory.instance.SmgAmmo.ToString();
                    break;
                case WeaponType.Pistola:
                    amountOfAmmo.text = Inventory.instance.PistolAmmo.ToString();
                    break;
                default:
                    amountOfAmmo.text = "0";
                    break;
            }
        }
        else
        {
            amountOfAmmo.text = "0";
        }
    }

    public void ShowItemsMenu(bool show)
    {
        itemsToUseMenu.SetActive(show);
    }

    public void ClearItemsMenu()
    {
        foreach (Transform itemSlot in itemSlotContainerParent)
        {
            Destroy(itemSlot.gameObject);
        }
    }

    public void CreateItemButton(ItemManager item)
    {
        RectTransform itemSlot = Instantiate(itemSlotContainerPrefab, itemSlotContainerParent).GetComponent<RectTransform>();

        Image itemImage = itemSlot.Find("Item image").GetComponent<Image>();
        itemImage.sprite = item.Icon;

        Text itemsAmountText = itemSlot.Find("Amount Text").GetComponent<Text>();
        if (item.Amount > 1)
            itemsAmountText.text = item.Amount.ToString();
        else
            itemsAmountText.text = "";

        itemSlot.GetComponent<ItemButton>().itemOnButton = item;
    }

    public void SelectItem(ItemManager itemToUse)
    {
        itemName.text = itemToUse.ItemName;
        itemDescriptionText.text = itemToUse.ItemDescription;
        itemDescription.SetActive(true);
    }

    public IEnumerator ShowDamageEffect(int damage, bool healing, bool isPlayerAttacking)
    {
        if (!healing)
        {
            damageDealt.color = Color.red;
            damageReceived.color = Color.red;
            if (isPlayerAttacking) 
            {
                damageDealt.text = "-" + damage.ToString();
                yield return new WaitForSeconds(5f);
                damageDealt.text = string.Empty;
            }
            else 
            {
                damageReceived.text = "-" + damage.ToString();
                yield return new WaitForSeconds(5f);
                damageReceived.text = string.Empty;
            }
        }
        else
        {
            damageDealt.color = Color.green;
            damageReceived.color = Color.green;
            if (!isPlayerAttacking) 
            {
                damageDealt.text = "+" + damage.ToString();
                yield return new WaitForSeconds(5f);
                damageDealt.text = string.Empty;
            }
            else 
            {
                damageReceived.text = "+" + damage.ToString();
                yield return new WaitForSeconds(5f);
                damageReceived.text = string.Empty;
            }
        }
    }

    public IEnumerator ShowLog(string newText)
    {
        log.text = newText;
        yield return new WaitForSeconds(2f);
    }

    public IEnumerator Shake(Rigidbody2D rb)
    {
        Vector2 originalPos = rb.position;
        int directionX;
        int directionY;

        for (int i = 0; i < 5; i++)
        {
            directionY = UnityEngine.Random.Range(-1, 2);
            directionX = UnityEngine.Random.Range(-1, 2);

            rb.velocity = new Vector2(directionX, directionY) * 10;
            yield return new WaitForSeconds(0.05f);
            rb.velocity = Vector2.zero;
            rb.position = originalPos;
        }
    }
}
