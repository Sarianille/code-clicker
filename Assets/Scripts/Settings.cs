using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Sprite keyboardVer1;
    [SerializeField]
    private Sprite keyboardVer2;
    [SerializeField]
    private Sprite keyboardVer3;
    [SerializeField]
    private Sprite keyboardVer4;
    [SerializeField]
    private Sprite keyboardVer5;
    [SerializeField]
    private Sprite keyboardVer6;
    [SerializeField]
    private Button keyboardButton;

    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject scrollView;
    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Sprite backgroundVer1;
    [SerializeField]
    private Sprite backgroundVer2;
    [SerializeField]
    private Sprite backgroundVer3;
    [SerializeField]
    private Sprite backgroundVer4;
    [SerializeField]
    private Sprite backgroundVer5;
    [SerializeField]
    private Sprite backgroundVer6;
    [SerializeField]
    private Image backgroundImage;

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

    public void OpenMenu()
    {
        if (menu != null)
        {
            bool isActive = menu.activeSelf;
            menu.SetActive(!isActive);
            scrollView.SetActive(isActive);
        }
    }

    public void OpenSetting()
    {
        if (settings !=  null)
        {
            bool isActive = settings.activeSelf;
            settings.SetActive(!isActive);
            menu.SetActive(isActive);
        }
    }
}
