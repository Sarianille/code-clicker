using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    Building building;
    int MinAmount;
    ulong MinLOC;
    Condition DependantOn;

    public Condition(Building building, int MinAmount, ulong MinLOC, Condition DependantOn)
    {
        this.building = building;
        this.MinAmount = MinAmount;
        this.MinLOC = MinLOC;
        this.DependantOn = DependantOn;
    }

    public bool IsMet()
    {
        if (DependantOn != null)
        {
            if (DependantOn.IsMet())
            {
                return building.Amount >= MinAmount && building.clicker.overallLOCCount >= MinLOC;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return building.Amount >= MinAmount && building.clicker.overallLOCCount >= MinLOC;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
