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

    [SerializeField]
    private GameObject energyDrink;

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

    private void UpdateOwnedBuildings() => ownedBuildings = clicker.buildings.Where(building => building.GetAmount() > 0).ToList();
    private void UpdateBuildingMultiplier() => ownedBuildings[currentBuilding].Multiplier = 1 + ((float)(10 * ownedBuildings[currentBuilding].GetAmount()) / 100);
    private void HideEnergyDrink() => energyDrink.transform.localPosition = offSreenPosition;

    private void DropEnergyDrink()
    {
        var appearNext = Random.Range(dropFrequencyMin, dropFrequencyMax);
        Invoke(nameof(DropEnergyDrink), appearNext);
        Invoke(nameof(HideEnergyDrink), timerDisappear);
        ChooseBoost();
    }

    private void ChooseBoost()
    {
        currentBoost = Random.Range(0, 4);

        ShowEnergyDrink();
    }

    private void ShowEnergyDrink()
    {
        float xPosition = Random.Range(0 - xRange, 0 + xRange);
        float yPosition = Random.Range(0 - yRange, 0 + yRange);

        randomPosition = new Vector3(xPosition, yPosition, 0);

        energyDrink.transform.localPosition = randomPosition;
    }

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

    private void Add15Percent()
    {
        clicker.ManageLOCServerRpc((ulong)(clicker.currentLOCCount * 0.15), false);
        clicker.notification.ShowMessage("Energy drink: +15% of your LOC");
    }

    private void IncreasedProduction()
    {
        clicker.ProductionMultiplier = 10;
        clicker.notification.ShowMessage("Energy drink: Production x10");

        Invoke(nameof(StopBoost), 42);
    }

    private void BuildingSpecial()
    {
        UpdateOwnedBuildings();
        currentBuilding = Random.Range(0, ownedBuildings.Count);
        clicker.notification.ShowMessage("Energy drink: Building special");

        InvokeRepeating(nameof(UpdateBuildingMultiplier), 0, 1);
        Invoke(nameof(StopBoost), 30);
    }

    private void IncreasedClicking()
    {
        clicker.ClickMultiplier = 420;
        clicker.notification.ShowMessage("Energy drink: Clicking x420");

        Invoke(nameof(StopBoost), 15);
    }

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
