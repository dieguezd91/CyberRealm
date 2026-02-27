using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject UI;
    public GameObject mainMenu;

    public Image lifeBar;
    public Image magicBar;
    public Image XPBar;
    public Text credits;
    public Text level;

    public void UpdateStatBars(float currentLife, float maxLife, float currentMagic, float maxMagic, float currentXP, float maxXP)
    {
        lifeBar.fillAmount = currentLife/maxLife;
        magicBar.fillAmount = currentMagic / maxMagic;
        XPBar.fillAmount = currentXP / maxXP;
    }

    public void UpdateCredits(int currentCredits)
    {
        credits.text = currentCredits.ToString();
    }

    public void UpdateLevel(int currentLevel)
    {
        level.text = "Level: " + currentLevel.ToString();
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        UI.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        UI.SetActive(true);
    }

    public void OpenOptionsMenu()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        else if (mainMenu != null) mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseOptionsMenu()
    {
        if (pauseMenu != null) pauseMenu.SetActive(true);
        else if (mainMenu != null) mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
}
