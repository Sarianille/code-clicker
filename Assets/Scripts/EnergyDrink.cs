using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnergyDrink : MonoBehaviour
{
    private float dropFrequencyMin = 300;
    private float dropFrequencyMax = 900;

    private float timerDisappear = 20;

    Vector3 offSreenPosition = new Vector3 (-1060, 0, 0);
    Vector3 randomPosition;
    private float xRange = 845;
    private float yRange = 370;

    [SerializeField] private GameObject energyDrink;

    public Clicker clicker;
    private List<Building> ownedBuildings;

    private int currentBoost;
    private int currentBuilding;

    void Start()
    {
        energyDrink.transform.localPosition = offSreenPosition;

        var appearNext = Random.Range(dropFrequencyMin, dropFrequencyMax);
        Invoke(nameof(DropEnergyDrink), appearNext);
    }

    /// <summary>
    /// Updates the list of owned buildings.
    /// </summary>
    private void UpdateOwnedBuildings() => ownedBuildings = clicker.buildings.Where(building => building.GetAmount() > 0).ToList();
    /// <summary>
    /// Changes the multiplier of the building with the boost. Adjusts according to the amount of buildings.
    /// </summary>
    private void UpdateBuildingMultiplier() => ownedBuildings[currentBuilding].Multiplier = 1 + ((float)(10 * ownedBuildings[currentBuilding].GetAmount()) / 100);
    /// <summary>
    /// Moves the energy drink off screen.
    /// </summary>
    private void HideEnergyDrink() => energyDrink.transform.localPosition = offSreenPosition;

    /// <summary>
    /// As soon as the energy drink appears, calculates the next time it will appear
    /// If not clicked, it will disappear after 20 seconds.
    /// </summary>
    private void DropEnergyDrink()
    {
        var appearNext = Random.Range(dropFrequencyMin, dropFrequencyMax);
        Invoke(nameof(DropEnergyDrink), appearNext);
        Invoke(nameof(HideEnergyDrink), timerDisappear);
        ChooseBoost();
    }

    /// <summary>
    /// Randomizes the boost.
    /// </summary>
    private void ChooseBoost()
    {
        currentBoost = Random.Range(0, 4);

        ShowEnergyDrink();
    }

    /// <summary>
    /// Randomizes the position of the energy drink on screen.
    /// </summary>
    private void ShowEnergyDrink()
    {
        float xPosition = Random.Range(0 - xRange, 0 + xRange);
        float yPosition = Random.Range(0 - yRange, 0 + yRange);

        randomPosition = new Vector3(xPosition, yPosition, 0);

        energyDrink.transform.localPosition = randomPosition;
    }

    /// <summary>
    /// Upon clicking on the energy drink, hides it and starts the boost.
    /// </summary>
    public void StartBoost()
    {
        CancelInvoke(nameof(HideEnergyDrink));
        HideEnergyDrink();

        switch (currentBoost)
        {
            case 0:
                Add15Percent();
                break;
            case 1:
                IncreasedProduction();
                break;
            case 2:
                BuildingSpecial();
                break;
            case 3:
                IncreasedClicking();
                break;
        }
    }

    /// <summary>
    /// Adds 15% of the current LOC count.
    /// </summary>
    private void Add15Percent()
    {
        clicker.ManageLOCServerRpc((ulong)(clicker.currentLOCCount * 0.15), false);
        clicker.notification.ShowMessage("Energy drink: +15% of your LOC");
    }

    /// <summary>
    /// Increases the production of all buildings by 10x for 42 seconds.
    /// </summary>
    private void IncreasedProduction()
    {
        clicker.ProductionMultiplier = 10;
        clicker.notification.ShowMessage("Energy drink: Production x10");

        Invoke(nameof(StopBoost), 42);
    }

    /// <summary>
    /// Increases the production of a random building based on the amount of that building type for 30 seconds.
    /// </summary>
    private void BuildingSpecial()
    {
        UpdateOwnedBuildings();
        currentBuilding = Random.Range(0, ownedBuildings.Count);
        clicker.notification.ShowMessage("Energy drink: Building special");

        InvokeRepeating(nameof(UpdateBuildingMultiplier), 0, 1);
        Invoke(nameof(StopBoost), 30);
    }

    /// <summary>
    /// Increases the clicking multiplier by 420x for 15 seconds.
    /// </summary>
    private void IncreasedClicking()
    {
        clicker.ClickMultiplier = 420;
        clicker.notification.ShowMessage("Energy drink: Clicking x420");

        Invoke(nameof(StopBoost), 15);
    }

    /// <summary>
    /// Stops the boost by reseting the multipliers and stops possible invoked methords.
    /// </summary>
    private void StopBoost()
    {
        clicker.ProductionMultiplier = 1;
        clicker.ClickMultiplier = 1;
        ownedBuildings[currentBuilding].Multiplier = 1;
        if (IsInvoking(nameof(UpdateBuildingMultiplier)))
        {
            CancelInvoke(nameof(UpdateBuildingMultiplier));
        }
    }
}
