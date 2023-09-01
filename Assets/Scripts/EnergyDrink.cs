using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDrink : MonoBehaviour
{
    private float dropFrequencyMin = 300;
    private float dropFrequencyMax = 900;

    private float timerAppear;
    private float timerDisappear = 20;

    Vector3 offSreenPosition = new Vector3 (-1060, 0, 0);
    Vector3 randomPosition;
    private float xRange = 845;
    private float yRange = 370;

    [SerializeField]
    private GameObject energyDrink;

    public Clicker clicker;
    public List<Building> ownedBuildings;

    private int currentBoost;
    private int currentBuilding;
    private float productionBoostTime = 42;
    private float buildingSpecialBoostTime = 30;
    private float clickingBoostTime = 15;

    // Start is called before the first frame update
    void Start()
    {
        energyDrink.transform.localPosition = offSreenPosition;

        timerAppear = Random.Range(dropFrequencyMin, dropFrequencyMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOffScreen())
        {
            timerAppear -= Time.deltaTime;
        }
        else
        {
            timerDisappear -= Time.deltaTime;

            if (timerDisappear <= 0)
            {
                energyDrink.transform.localPosition = offSreenPosition;

                timerDisappear = 20;
            }
        }

        if (timerAppear <= 0)
        {
            timerAppear = Random.Range(dropFrequencyMin, dropFrequencyMax);
            ChooseBoost();
        }
    }

    private void UpdateOwnedBuildings()
    {
        ownedBuildings = clicker.buildings.Where(building => building.Amount > 0).ToList();
    }

    private bool IsOffScreen()
    {
        return energyDrink.transform.localPosition == offSreenPosition;
    }

    private void ShowEnergyDrink()
    {
        float xPosition = Random.Range(0 - xRange, 0 + xRange);
        float yPosition = Random.Range(0 - yRange, 0 + yRange);

        randomPosition = new Vector3(xPosition, yPosition, 0);

        energyDrink.transform.localPosition = randomPosition;
    }

    private void ChooseBoost()
    {
        currentBoost = Random.Range(0, 4);

        ShowEnergyDrink();
    }

    public void StartBoost()
    {
        switch (currentBoost)
        {
            case 0:
                Add15Percent();
                break;
            case 1:
                InvokeRepeating("IncreasedProduction", 0, 1);
                break;
            case 2:
                InvokeRepeating("BuildingSpecial", 0, 1);
                break;
            case 3:
                InvokeRepeating("IncreasedClicking", 0, 1);
                break;
        }

        energyDrink.transform.localPosition = offSreenPosition;
    }

    private void Add15Percent()
    {
        clicker.IncrementLOC((ulong)(clicker.currentLOCCount * 0.15));
        clicker.notification.ShowMessage("Energy drink: +15% of your LOC");
    }

    private void IncreasedProduction()
    {
        if (productionBoostTime == 42)
        {
            clicker.ProductionMultiplier = 10;
            clicker.notification.ShowMessage("Energy drink: Production x10");
        }

        --productionBoostTime;

        if (productionBoostTime == 0) 
        {
            CancelInvoke("IncreasedProduction");
            productionBoostTime = 42;

            clicker.ProductionMultiplier = 1;
        }
    }

    private void BuildingSpecial()
    {
        if (buildingSpecialBoostTime == 30)
        {
            UpdateOwnedBuildings();
            currentBuilding = Random.Range(0, ownedBuildings.Count);
            clicker.notification.ShowMessage("Energy drink: Building special");
        }

        ownedBuildings[currentBuilding].Multiplier += ((float)(10 * ownedBuildings[currentBuilding].Amount) / 100);

        --buildingSpecialBoostTime;

        if (buildingSpecialBoostTime == 0)
        {
            CancelInvoke("BuildingSpecial");
            buildingSpecialBoostTime = 30;

            ownedBuildings[currentBuilding].Multiplier = 1;
        }
    }

    private void IncreasedClicking()
    {
        if (clickingBoostTime == 15)
        {
            clicker.ClickMultiplier = 420;
            clicker.notification.ShowMessage("Energy drink: Clicking x420");
        }
        
        --clickingBoostTime;

        if (clickingBoostTime == 0)
        {
            CancelInvoke("IncreasedClicking");
            clickingBoostTime = 15;

            clicker.ClickMultiplier = 1;
        }
    }
}
