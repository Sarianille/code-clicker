using System.Collections.Generic;
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
    public Key key;
    public URandom urandom;
    public CodeMonkey codeMonkey;
    public GiftedChild giftedChild;
    public MFFStudent MFFStudent;
    public TeamMember teamMember;
    public ContractorTeam contractorTeam;
    public Company company;
    public AI ai;
    public Building[] buildings;
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
        buildings = new Building[] { key, urandom, codeMonkey, giftedChild, MFFStudent, teamMember, contractorTeam, company, ai };
        InvokeRepeating("AddBuilding", 0, 1);
        timerAppear = Random.Range(dropFrequencyMin, dropFrequencyMax);
    }

    // Update is called once per frame
    void Update()
    {
        timerAppear -= Time.deltaTime;
        timerDisappear -= Time.deltaTime;

        if (!IsOffScreen() && timerDisappear <= 0)
        {
            energyDrink.transform.localPosition = offSreenPosition;

            timerDisappear = 20;
        }

        if (timerAppear <= 0)
        {
            timerAppear = Random.Range(dropFrequencyMin, dropFrequencyMax);
            ChooseBoost();
        }
    }

    private void AddBuilding()
    {
        foreach (var building in buildings) 
        {
            if (building.Amount > 0 && !ownedBuildings.Contains(building))
            {
                ownedBuildings.Add(building);
            }
        }

        if (ownedBuildings.Count == buildings.Length)
        {
            CancelInvoke("AddBuilding");
        }
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
    }

    private void IncreasedProduction()
    {
        if (productionBoostTime == 42)
        {
            clicker.ProductionMultiplier = 10;
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
            currentBuilding = Random.Range(0, ownedBuildings.Count);
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
