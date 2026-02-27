using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemButton : MonoBehaviour
{

    public ItemManager itemOnButton;
    
    public void Press()
    {
        if(BattleManager.instance.IsBattleActive && BattleManager.instance.View.itemsToUseMenu.activeInHierarchy)
        {
            BattleManager.instance.SelectedItemToUse(itemOnButton);
        }
        else
        {
            MenuManager.instance.ItemName.text = itemOnButton.ItemName;
            MenuManager.instance.ItemDescription.text = itemOnButton.ItemDescription;
            MenuManager.instance.activeItem = itemOnButton;
            MenuManager.instance.itemsDescription.SetActive(true);
        }
    }
}
