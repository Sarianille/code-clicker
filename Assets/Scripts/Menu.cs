using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject shop;
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject buildings;
    [SerializeField] private GameObject upgrades;
    [SerializeField] private GameObject achievements;
    [SerializeField] private GameObject multiplayer;
    [SerializeField] private GameObject host;
    [SerializeField] private GameObject join;
    private GameObject[] sidePanels;

    [SerializeField] private Button keyboardButton;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite[] keyboardVersions;
    private int currentKeyboardVersion = 0;
    [SerializeField] private Sprite[] backgroundVersions;
    private int currentBackgroundVersion = 0;

    void Start()
    {
        sidePanels = new GameObject[] { settings, achievements, multiplayer, host, join };

        SetupKeyboard(currentKeyboardVersion);
        SetupBackground(currentBackgroundVersion);
        SetupButtons();
    }

    /// <summary>
    /// Changes the keyboard image.
    /// </summary>
    /// <param name="version">Placement of the image in the sprites array.</param>
    private void SetupKeyboard(int version) => keyboardButton.image.sprite = keyboardVersions[version];

    /// <summary>
    /// Changes the background image.
    /// </summary>
    /// <param name="version">Placement of the image in the sprites array.</param>
    private void SetupBackground(int version) => backgroundImage.sprite = backgroundVersions[version];

    /// <summary>
    /// Switches to the next keyboard image in the array.
    /// </summary>
    public void SelecNexttKeyboard()
    {
        currentKeyboardVersion = SelectNext(currentKeyboardVersion);
        SetupKeyboard(currentKeyboardVersion);
    }

    /// <summary>
    /// Switches to the previous keyboard image in the array.
    /// </summary>
    public void SelectPreviousKeyboard()
    {
        currentKeyboardVersion = SelectPrevious(currentKeyboardVersion);
        SetupKeyboard(currentKeyboardVersion);
    }

    /// <summary>
    /// Switches to the next background image in the array.
    /// </summary>
    public void SelectNextBackground()
    {
        currentBackgroundVersion = SelectNext(currentBackgroundVersion);
        SetupBackground(currentBackgroundVersion);
    }

    /// <summary>
    /// Switches to the previous background image in the array.
    /// </summary>
    public void SelectPreviousBackground()
    {
        currentBackgroundVersion = SelectPrevious(currentBackgroundVersion);
        SetupBackground(currentBackgroundVersion);
    }

    /// <summary>
    /// Updates the current version of an image to the next one.
    /// </summary>
    /// <param name="currentVersion">Current version of the image.</param>
    /// <returns></returns>
    private int SelectNext(int currentVersion)
    {
        currentVersion++;

        if (currentVersion > 5)
        {
            currentVersion = 0;
        }

        return currentVersion;
    }

    /// <summary>
    /// Updates the current version of an image to the previous one.
    /// </summary>
    /// <param name="currentVersion">Current version of the image.</param>
    /// <returns></returns>
    private int SelectPrevious(int currentVersion)
    {
        currentVersion--;

        if (currentVersion < 0)
        {
            currentVersion = 5;
        }

        return currentVersion;
    }

    /// <summary>
    /// Displays the correct panel when the corresponding button is clicked.
    /// Disables the button of the active panel to avoid confusion.
    /// </summary>
    public void SwitchBetweenBuildingsAndUpgrades()
    {
        if (buildings != null && upgrades != null)
        {
            bool isActive = buildings.activeSelf;
            buildings.SetActive(!isActive);
            upgrades.SetActive(isActive);

            shop.transform.Find("Shop Buttons/Buildings").gameObject.GetComponent<Button>().interactable = isActive;
            shop.transform.Find("Shop Buttons/Upgrades").gameObject.GetComponent<Button>().interactable = !isActive;
        }
    }

    /// <summary>
    /// Opens the menu and closes any other panel.
    /// If the menu is already open, it closes it and opens the shop.
    /// </summary>
    public void OpenMenu()
    {
        if (menu != null)
        {
            var activePanels = sidePanels.Where(panel => panel.activeSelf).ToList();

            // In case the player wants to access the menu from a different panel than the shop
            if (activePanels.Count > 0)
            {
                foreach (var panel in activePanels)
                {
                    panel.SetActive(false);
                }
            }
            bool isActive = menu.activeSelf;
            menu.SetActive(!isActive);
            shop.SetActive(isActive);
        }
    }

    /// <summary>
    /// Opens the panel and closes the parent panel or vice versa.
    /// </summary>
    /// <param name="panel">Panel whose visibility we're changing.</param>
    /// <param name="parent">The panel that was displayed before/should be displayed after.</param>
    private void OpenPanel(GameObject panel, GameObject parent)
    {
        if (panel != null && parent != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            parent.SetActive(isActive);
        }
    }

    /// <summary>
    /// Assigns the correct functions to the buttons from the menu.
    /// </summary>
    public void SetupButtons()
    {
        menu.transform.Find("Settings").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(settings, menu));
        menu.transform.Find("Achievements").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(achievements, menu));
        menu.transform.Find("Multiplayer").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(multiplayer, menu));
        settings.transform.Find("Back").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(settings, menu));
        multiplayer.transform.Find("BackToMenu").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(multiplayer, menu));
        multiplayer.transform.Find("Host").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(host, multiplayer));
        host.transform.Find("BackToMultiplayer").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(host, multiplayer));
        multiplayer.transform.Find("Join").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(join, multiplayer));
        join.transform.Find("BackToMultiplayer").gameObject.GetComponent<Button>().onClick.AddListener(() => OpenPanel(join, multiplayer));
    }

}
