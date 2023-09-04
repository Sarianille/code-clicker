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

        panel.SetActive(false);
    }

    public void Information()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }
}
