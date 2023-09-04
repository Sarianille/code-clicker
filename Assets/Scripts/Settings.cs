using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite keyboardVer1;
    [SerializeField] private Sprite keyboardVer2;
    [SerializeField] private Sprite keyboardVer3;
    [SerializeField] private Sprite keyboardVer4;
    [SerializeField] private Sprite keyboardVer5;
    [SerializeField] private Sprite keyboardVer6;
    [SerializeField] private Button keyboardButton;

    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject buildings;
    [SerializeField] private GameObject upgrades;
    [SerializeField] private GameObject achievements;
    [SerializeField] private GameObject multiplayer;

    [SerializeField] private Button buildingsButton;
    [SerializeField] private Button upgradesButton;
    [SerializeField] private Button achievementsButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button multiplayerButton;
    [SerializeField] private Button backMenuButton;

    [SerializeField] private Sprite backgroundVer1;
    [SerializeField] private Sprite backgroundVer2;
    [SerializeField] private Sprite backgroundVer3;
    [SerializeField] private Sprite backgroundVer4;
    [SerializeField] private Sprite backgroundVer5;
    [SerializeField] private Sprite backgroundVer6;
    [SerializeField] private Image backgroundImage;

    [SerializeField] private GameObject hostButton;
    [SerializeField] private GameObject joinButton;
    [SerializeField] private GameObject publicHostButton;
    [SerializeField] private GameObject privateHostButton;
    [SerializeField] private GameObject privateHostText;
    [SerializeField] private GameObject publicJoinButton;
    [SerializeField] private GameObject privateJoinText;
    [SerializeField] private GameObject privateJoinInput;
    [SerializeField] private GameObject backMultiplayer;
    [SerializeField] private GameObject startGame;
    [SerializeField] private GameObject playerAmount;

    class Keyboard
    {
        public Sprite[] versions;
        public int currentKeyboardVersion = 0;
    }

    class Background
    {
        public Sprite[] versions;
        public int currentBackgroundVersion = 0;
    }

    private Keyboard keyboard = new Keyboard();
    private Background background = new Background();

    // Start is called before the first frame update
    void Start()
    {
        SetupGraphics();
        SetupKeyboard(keyboard.currentKeyboardVersion);
        SetupBackground(background.currentBackgroundVersion);
        SetupButtons();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupGraphics()
    {
        keyboard.versions = new Sprite[] { keyboardVer1, keyboardVer2, keyboardVer3, keyboardVer4, keyboardVer5, keyboardVer6 };
        background.versions = new Sprite[] { backgroundVer1, backgroundVer2, backgroundVer3, backgroundVer4, backgroundVer5, backgroundVer6 };
    }

    public void SelecNexttKeyboard()
    {
        keyboard.currentKeyboardVersion++;

        if (keyboard.currentKeyboardVersion > 5)
        {
            keyboard.currentKeyboardVersion = 0;
        }

        SetupKeyboard(keyboard.currentKeyboardVersion);
    }

    public void SelectPreviousKeyboard()
    {
        keyboard.currentKeyboardVersion--;

        if (keyboard.currentKeyboardVersion < 0)
        {
            keyboard.currentKeyboardVersion = 5;
        }

        SetupKeyboard(keyboard.currentKeyboardVersion);
    }

    public void SetupKeyboard(int version)
    {
        keyboardButton.image.sprite = keyboard.versions[version];
    }

    public void SelectNextBackground()
    {
        background.currentBackgroundVersion++;

        if (background.currentBackgroundVersion > 5)
        {
            background.currentBackgroundVersion = 0;
        }

        SetupBackground(background.currentBackgroundVersion);
    }

    public void SelectPreviousBackground()
    {
        background.currentBackgroundVersion--;

        if (background.currentBackgroundVersion < 0)
        {
            background.currentBackgroundVersion = 5;
        }

        SetupBackground(background.currentBackgroundVersion);
    }

    public void SetupBackground(int version)
    {
        backgroundImage.sprite = background.versions[version];
    }

    public void SwitchBetweenBuildingsAndUpgrades()
    {
        if (buildings != null && upgrades != null)
        {
            bool isActive = buildings.activeSelf;
            buildings.SetActive(!isActive);
            upgrades.SetActive(isActive);

            buildingsButton.interactable = isActive;
            upgradesButton.interactable = !isActive;
        }
    }

    public void OpenMenu()
    {
        if (menu != null)
        {
            if (settings.activeSelf || achievements.activeSelf)
            {
                settings.SetActive(false);
                achievements.SetActive(false);
            }
            bool isActive = menu.activeSelf;
            menu.SetActive(!isActive);
            shop.SetActive(isActive);
        }
    }

    private void OpenPanel(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            menu.SetActive(isActive);
        }
    }

    public void SetupButtons()
    {
        settingsButton.onClick.AddListener(() => OpenPanel(settings));
        achievementsButton.onClick.AddListener(() => OpenPanel(achievements));
        backButton.onClick.AddListener(() => OpenPanel(settings));
        multiplayerButton.onClick.AddListener(() => OpenPanel(multiplayer));
        backMenuButton.onClick.AddListener(() => OpenPanel(multiplayer));
    }

    public void OpenHost()
    {
        if (hostButton != null)
        {
            Hide();
            publicHostButton.SetActive(true);
            privateHostButton.SetActive(true);
            privateHostText.SetActive(true);
            backMultiplayer.SetActive(true);
            startGame.SetActive(true);
            playerAmount.SetActive(true);
        }
    }

    public void OpenJoin()
    {
        if (joinButton != null)
        {
            Hide();
            publicJoinButton.SetActive(true);
            privateJoinText.SetActive(true);
            privateJoinInput.SetActive(true);
            backMultiplayer.SetActive(true);
        }
    }

    private void Hide()
    {
        joinButton.SetActive(false);
        hostButton.SetActive(false);
    }

    public void BackMultiplayer()
    {
        backMultiplayer.SetActive(false);
        publicHostButton.SetActive(false);
        privateHostButton.SetActive(false);
        privateHostText.SetActive(false);
        publicJoinButton.SetActive(false);
        privateJoinText.SetActive(false);
        privateJoinInput.SetActive(false);
        startGame.SetActive(false);
        playerAmount.SetActive(false);
        joinButton.SetActive(true);
        hostButton.SetActive(true);
    }
}
