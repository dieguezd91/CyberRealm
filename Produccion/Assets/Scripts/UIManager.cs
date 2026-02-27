using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject uiContainer;
    [SerializeField] private GameObject mainMenu;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image magicBar;
    [SerializeField] private Image xpBar;
    [SerializeField] private Text creditsText;
    [SerializeField] private Text levelText;

    public GameObject PauseMenu => pauseMenu;
    public GameObject OptionsMenu => optionsMenu;
    public GameObject UiContainer => uiContainer;
    public GameObject MainMenu => mainMenu;

    public void UpdateStatBars(float currentHealth, float maxHealth, float currentMagic, float maxMagic, float currentXp, float maxXp)
    {
        healthBar.fillAmount = currentHealth / maxHealth;
        magicBar.fillAmount = currentMagic / maxMagic;
        xpBar.fillAmount = currentXp / maxXp;
    }

    public void UpdateCredits(int currentCredits)
    {
        creditsText.text = currentCredits.ToString();
    }

    public void UpdateLevel(int currentLevel)
    {
        levelText.text = "Level: " + currentLevel.ToString();
    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        uiContainer.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        uiContainer.SetActive(true);
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
