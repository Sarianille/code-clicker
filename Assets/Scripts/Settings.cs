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
    private Button keyboard;
    private int currentKeyboardVersion = 0;
    [SerializeField]
    private GameObject settings;

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
    private int currentBackgroundVersion = 0;
    [SerializeField]
    private Image background;

    // Start is called before the first frame update
    void Start()
    {
        SetupKeyboard(currentKeyboardVersion);
        SetupBackground(currentBackgroundVersion);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelecNexttKeyboard()
    {
        currentKeyboardVersion++;

        if (currentKeyboardVersion > 5)
        {
            currentKeyboardVersion = 0;
        }

        SetupKeyboard(currentKeyboardVersion);
    }

    public void SetupKeyboard(int version)
    {
        Sprite[] versions = { keyboardVer1, keyboardVer2, keyboardVer3, keyboardVer4, keyboardVer5, keyboardVer6 };

        keyboard.image.sprite = versions[version];
    }

    public void SelectNextBackground()
    {
        currentBackgroundVersion++;

        if (currentBackgroundVersion > 5)
        {
            currentBackgroundVersion = 0;
        }

        SetupBackground(currentBackgroundVersion);
    }

    public void SetupBackground(int version)
    {
        Sprite[] versions = { backgroundVer1, backgroundVer2, backgroundVer3, backgroundVer4, backgroundVer5, backgroundVer6 };

        background.sprite = versions[version];
    }

    public void OpenSetting()
    {
        if (settings !=  null)
        {
            bool isActive = settings.activeSelf;
            settings.SetActive(!isActive);
        }
    }
}
