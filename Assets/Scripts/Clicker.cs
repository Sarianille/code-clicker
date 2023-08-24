using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    private int overallLOCCount = 0;
    private int currentLOCCount = 0;
    [SerializeField]
    private TMP_Text LOCCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementLOC()
    {
        overallLOCCount++;
        currentLOCCount++;
        LOCCount.text = currentLOCCount.ToString();
    }
}
