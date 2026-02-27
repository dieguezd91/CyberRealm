using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatButtonsHandler : MonoBehaviour
{
    public Button runButton;
    public Button rangeButton;
    public Image meleeButtonImage;
    public Image rangeButtonImage;

    [SerializeField] Sprite fistSprite;
    [SerializeField] Sprite knifeSprite;
    [SerializeField] Sprite batSprite;
    [SerializeField] Sprite katanaSprite;
    [SerializeField] Sprite pistolSprite;
    [SerializeField] Sprite SMGSprite;
    [SerializeField] Sprite shotgunSprite;

    void Update()
    {
        EnableOrDisableButtons();
        ChangeAttackButtonsSprite();
    }


    void EnableOrDisableButtons()
    {
        if (BattleManager.instance.bossBattle || BattleManager.instance.dinniesBattle || GameManager.instance.tutorial)
            runButton.interactable = false;
        else if (BattleManager.instance.randomBattle)
            runButton.interactable = true;

        if (PlayerStats.instance.equipedRangeWeapon != null && Inventory.instance.hasAmmo == true) rangeButton.interactable = true;
        else rangeButton.interactable = false;
    }


    void ChangeAttackButtonsSprite()
    {
        if (PlayerStats.instance.equipedMeleeWeapon == null)
        {
            meleeButtonImage.sprite = fistSprite;
        }
        else
        {
            switch (PlayerStats.instance.equipedMeleeWeapon.itemName)
            {
                case "Cuchillo":
                    meleeButtonImage.sprite = knifeSprite;
                    break;
                case "Bate":
                    meleeButtonImage.sprite = batSprite;
                    break;
                case "Katana":
                    meleeButtonImage.sprite = katanaSprite;
                    break;
            }
        }
        meleeButtonImage.SetNativeSize();

        if(PlayerStats.instance.equipedRangeWeapon == null)
        {
            rangeButtonImage.sprite = null;
            rangeButtonImage.color = Color.clear;
        }
        else
        {
            rangeButtonImage.color = Color.white;
            switch (PlayerStats.instance.equipedRangeWeapon.itemName)
            {
                case "Pistola":
                    rangeButtonImage.sprite = pistolSprite;
                    break;
                case "Subfusil":
                    rangeButtonImage.sprite = SMGSprite;
                    break;
                case "Escopeta":
                    rangeButtonImage.sprite = shotgunSprite;
                    break;
            }
            rangeButtonImage.SetNativeSize();
        }
    }
}
