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
        if (BattleManager.instance.BossBattle || BattleManager.instance.IsDinnieBattle || GameManager.instance.Tutorial)
            runButton.interactable = false;
        else if (BattleManager.instance.RandomBattle)
            runButton.interactable = true;

        if (PlayerStats.instance.EquippedRangeWeapon != null && Inventory.instance.HasAmmo == true) rangeButton.interactable = true;
        else rangeButton.interactable = false;
    }


    void ChangeAttackButtonsSprite()
    {
        if (PlayerStats.instance.EquippedMeleeWeapon == null)
        {
            meleeButtonImage.sprite = fistSprite;
        }
        else
        {
            switch (PlayerStats.instance.EquippedMeleeWeapon.WeaponType)
            {
                case WeaponType.Cuchillo:
                    meleeButtonImage.sprite = knifeSprite;
                    break;
                case WeaponType.Bate:
                    meleeButtonImage.sprite = batSprite;
                    break;
                case WeaponType.Katana:
                    meleeButtonImage.sprite = katanaSprite;
                    break;
            }
        }
        meleeButtonImage.SetNativeSize();

        if(PlayerStats.instance.EquippedRangeWeapon == null)
        {
            rangeButtonImage.sprite = null;
            rangeButtonImage.color = Color.clear;
        }
        else
        {
            rangeButtonImage.color = Color.white;
            switch (PlayerStats.instance.EquippedRangeWeapon.WeaponType)
            {
                case WeaponType.Pistola:
                    rangeButtonImage.sprite = pistolSprite;
                    break;
                case WeaponType.Subfusil:
                    rangeButtonImage.sprite = SMGSprite;
                    break;
                case WeaponType.Escopeta:
                    rangeButtonImage.sprite = shotgunSprite;
                    break;
            }
            rangeButtonImage.SetNativeSize();
        }
    }
}
