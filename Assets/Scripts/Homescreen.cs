using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Homescreen : MonoBehaviour
{
    public Relay relay;
    public GameObject panel;

    /// <summary>
    ///  Starts a singleplayer session as a multiplayer session that no one can join.
    ///  This is a workaround so that we can ensure all objects are shown in both singleplayer and multiplayer
    ///  Hides the homescreen.
    /// </summary>
    public async void StartGame()
    {
        await relay.CreateRelay(1);

        ChangeHomescreenVisibility();
    }

    /// <summary>
    /// Toggles the homescreen visibility.
    /// </summary>
    public void ChangeHomescreenVisibility()
    {
        var isActive = panel.activeSelf;

        panel.SetActive(!isActive);
    }

    /// <summary>
    /// Toggles the information visibility.
    /// </summary>
    public void Information()
    {
        var isActive = panel.transform.Find("Information").gameObject.activeSelf;

        panel.transform.Find("StartGame").gameObject.SetActive(!isActive);
        panel.transform.Find("Information").gameObject.SetActive(!isActive);
        panel.transform.Find("Exit").gameObject.SetActive(!isActive);
        panel.transform.Find("InformationText").gameObject.SetActive(isActive);
    }

    /// <summary>
    /// Exits the game.
    /// </summary>
    public void Exit()
    {
        Application.Quit();
    }
}
