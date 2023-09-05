using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
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

    private void SetupKeyboard(int version) => keyboardButton.image.sprite = keyboardVersions[version];
    private void SetupBackground(int version) => backgroundImage.sprite = backgroundVersions[version];

    public void SelecNexttKeyboard()
    {
        currentKeyboardVersion = SelectNext(currentKeyboardVersion);
        SetupKeyboard(currentKeyboardVersion);
    }

    public void SelectPreviousKeyboard()
    {
        currentKeyboardVersion = SelectPrevious(currentKeyboardVersion);
        SetupKeyboard(currentKeyboardVersion);
    }

    public void SelectNextBackground()
    {
        currentBackgroundVersion = SelectNext(currentBackgroundVersion);
        SetupBackground(currentBackgroundVersion);
    }

    public void SelectPreviousBackground()
    {
        currentBackgroundVersion = SelectPrevious(currentBackgroundVersion);
        SetupBackground(currentBackgroundVersion);
    }

    private int SelectNext(int currentVersion)
    {
        currentVersion++;

        if (currentVersion > 5)
        {
            currentVersion = 0;
        }

        return currentVersion;
    }

    private int SelectPrevious(int currentVersion)
    {
        currentVersion--;

        if (currentVersion < 0)
        {
            currentVersion = 5;
        }

        return currentVersion;
    }

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

    public void OpenMenu()
    {
        if (menu != null)
        {
            var activePanels = sidePanels.Where(panel => panel.activeSelf).ToList();

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

    private void OpenPanel(GameObject panel, GameObject parent)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;
            panel.SetActive(!isActive);
            parent.SetActive(isActive);
        }
    }

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
