using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{

    public ItemsManager itemOnButton;
    
    public void Press()
    {
        if(BattleManager.instance.isBattleActive && BattleManager.instance.battleView.itemsToUseMenu.activeInHierarchy)
        {
            BattleManager.instance.SelectedItemToUse(itemOnButton);
        }
        else
        {
            MenuManager.instance.itemName.text = itemOnButton.itemName;
            MenuManager.instance.itemDescription.text = itemOnButton.itemDescription;
            MenuManager.instance.activeItem = itemOnButton;
            MenuManager.instance.itemsDescription.SetActive(true);
        }
    }
}
