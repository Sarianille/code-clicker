using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Homescreen : MonoBehaviour
{
    public Relay relay;
    public GameObject panel;

    public async void StartGame()
    {
        await relay.CreateRelay(1);

        ChangeHomescreenVisibility();
    }

    public void ChangeHomescreenVisibility()
    {
        var isActive = panel.activeSelf;

        panel.SetActive(!isActive);
    }

    public void Information()
    {
        var isActive = panel.transform.Find("Information").gameObject.activeSelf;

        panel.transform.Find("StartGame").gameObject.SetActive(!isActive);
        panel.transform.Find("Information").gameObject.SetActive(!isActive);
        panel.transform.Find("Exit").gameObject.SetActive(!isActive);
        panel.transform.Find("InformationText").gameObject.SetActive(isActive);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
